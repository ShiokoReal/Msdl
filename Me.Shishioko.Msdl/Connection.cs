using Me.Shishioko.Msdl.Data;
using Me.Shishioko.Msdl.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Numerics;
using System.Security.Cryptography;
using System.Text.Json;
using System.Text;
using System.Threading.Tasks;
using System.IO.Compression;
using System.Collections.Concurrent;
using System.Threading;
using System.Text.RegularExpressions;

namespace Me.Shishioko.Msdl
{
    public sealed partial class Connection
    {
        public event Action<string, ushort> OnAddress = (string Address, ushort Port) => { };
        public event Action<Exception?> OnDisconnect = (Exception? exception) => { };
        public event Func<ServerStatus> OnStatus = () => new ServerStatus();
        public event Action<string, Guid> OnLoginBegin = (string name, Guid guid) => { };
        public event Func<bool> OnEncryption = () => true;
        public event Action<JsonElement> OnEncryptionFailure = (JsonElement error) => { };
        public event Action<string, Guid, IEnumerable<Property>> OnEncryptionSuccess = (string name, Guid guid, IEnumerable<Property> properties) => { };
        public event Func<int> OnLoginCompression = () => 256;
        public event Func<(string, Guid, IEnumerable<Property>)> OnLoginEnd = () => (string.Empty, Guid.Empty, new List<Property>().AsEnumerable());
        public event Action<Client> OnPlay = (Client client) => { };
        public CompressionLevel CompressionLevel { get; set; } = CompressionLevel.Optimal;
        public TimeSpan Timeout { get; set; } = TimeSpan.FromSeconds(30);
        private DataStream Stream;
        private readonly ConcurrentQueue<byte[]> PacketQueue;
        private readonly SemaphoreSlim PacketSync;
        private int CompressionThreshold = -1;
        private bool Crashed = false;
        public Connection(Stream stream)
        {
            Stream = new(stream);
            PacketQueue = new();
            PacketSync = new(1, 1);
        }
        public void Initialize()
        {
            try
            {
                int next;
                using (DataStream packetIn = new(Receive()))
                {
                    if (packetIn.ReadS32V() != 0x00) throw new ProtocolViolationException("Expected handshake packet");
                    int version = packetIn.ReadS32V();
                    OnAddress.Invoke(packetIn.ReadStringS32V(), packetIn.ReadU16());
                    next = packetIn.ReadS32V();
                }
                if (next == 1)
                {
                    using (DataStream packetIn = new(Receive()))
                    {
                        if (packetIn.ReadS32V() != 0x00) throw new ProtocolViolationException();
                        using DataStream packetOut = new(new MemoryStream());
                        packetOut.WriteS32V(0x00);
                        packetOut.WriteStringS32V(JsonConvert.SerializeObject(OnStatus(), new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore }));
                        Send(packetOut.Get());
                    }
                    using (DataStream packetIn = new(Receive()))
                    {
                        if (packetIn.ReadS32V() != 0x01) throw new ProtocolViolationException();
                        using DataStream packetOut = new(new MemoryStream());
                        packetOut.WriteS32V(0x01);
                        packetOut.WriteU64(packetIn.ReadU64());
                        Send(packetOut.Get());
                    }
                }
                else if (next == 2)
                {
                    string name;
                    Guid guid;
                    using (DataStream packetIn = new(Receive()))
                    {
                        if (packetIn.ReadS32V() != 0x00) throw new ProtocolViolationException();
                        name = packetIn.ReadStringS32V();
                        if (!MinecraftUsernameRegex().IsMatch(name)) throw new ProtocolViolationException();
                        guid = packetIn.ReadGuid();
                        OnLoginBegin(name, guid);
                    }
                    if (OnEncryption())
                    {
                        string server = Convert.ToBase64String(RandomNumberGenerator.GetBytes(15));
                        byte[] verify = RandomNumberGenerator.GetBytes(4);
                        using RSA rsa = RSA.Create();
                        using (DataStream packetOut = new(new MemoryStream()))
                        {
                            packetOut.WriteS32V(0x01);
                            packetOut.WriteStringS32V(server);
                            packetOut.WriteU8AV(rsa.ExportSubjectPublicKeyInfo());
                            packetOut.WriteU8AV(verify);
                            Send(packetOut.Get());
                        }
                        using DataStream packetIn = new(Receive());
                        if (packetIn.ReadS32V() != 0x01) throw new ProtocolViolationException();
                        byte[] secret = rsa.Decrypt(packetIn.ReadU8AV(), RSAEncryptionPadding.Pkcs1);
                        if (!rsa.Decrypt(packetIn.ReadU8AV(), RSAEncryptionPadding.Pkcs1).SequenceEqual(verify)) throw new ProtocolViolationException();
                        if (FastAesStream.Supported) Stream = new(new FastAesStream(Stream.Stream, secret));
                        else Stream = new(new AesStream(Stream.Stream, secret));
                        BigInteger number = new(SHA1.HashData(Encoding.ASCII.GetBytes(server).Concat(secret).Concat(rsa.ExportSubjectPublicKeyInfo()).ToArray()).Reverse().ToArray());
                        string url = $"https://sessionserver.mojang.com/session/minecraft/hasJoined?username={name}&serverId={(number < 0 ? "-" + (-number).ToString("x") : number.ToString("x"))}";
                        using HttpClient http = new();
                        using HttpRequestMessage request = new(HttpMethod.Get, url);
                        using HttpResponseMessage response = http.Send(request);
                        using Stream stream = response.Content.ReadAsStream();
                        using JsonDocument auth = JsonDocument.Parse(stream);
                        if (response.StatusCode != HttpStatusCode.OK)
                        {
                            OnEncryptionFailure(auth.RootElement);
                            return;
                        }
                        name = auth.RootElement.GetProperty("name").GetString()!;
                        if (!MinecraftUsernameRegex().IsMatch(name)) throw new ProtocolViolationException();
                        guid = Guid.ParseExact(auth.RootElement.GetProperty("id").GetString()!, "N");
                        List<Property> authproperties = [];
                        foreach (JsonElement property in auth.RootElement.GetProperty("properties")!.EnumerateArray())
                        {
                            authproperties.Add(new(property.GetProperty("name").GetString()!, property.GetProperty("value").GetString()!, property.GetProperty("signature").GetString()));
                        }
                        OnEncryptionSuccess(name, guid, authproperties.AsReadOnly());
                    }
                    else
                    {
                        byte[] hash = MD5.HashData(Encoding.UTF8.GetBytes($"OfflinePlayer:{name}"));
                        hash[6] &= 0x0f;
                        hash[6] |= 0x30;
                        hash[8] &= 0x3f;
                        hash[8] |= 0x80;
                        guid = Guid.ParseExact(string.Concat(hash.Select(b => b.ToString("x2"))), "N");
                    }
                    int compression = OnLoginCompression();
                    using (DataStream packetOut = new(new MemoryStream()))
                    {
                        packetOut.WriteS32V(0x03);
                        packetOut.WriteS32V(compression);
                        Send(packetOut.Get());
                    }
                    CompressionThreshold = compression;
                    IEnumerable<Property> properties;
                    (name, guid, properties) = OnLoginEnd();
                    if (!MinecraftUsernameRegex().IsMatch(name)) throw new ArgumentException();
                    using (DataStream packetOut = new(new MemoryStream()))
                    {
                        packetOut.WriteS32V(0x02);
                        packetOut.WriteGuid(guid);
                        packetOut.WriteStringS32V(name);
                        packetOut.WriteS32V(properties.Count());
                        foreach (Property property in properties)
                        {
                            packetOut.WriteStringS32V(property.Name);
                            packetOut.WriteStringS32V(property.Value);
                            packetOut.WriteBool(property.Signature != null);
                            if (property.Signature != null) packetOut.WriteStringS32V(property.Signature);
                        }
                        Send(packetOut.Get());
                    }
                    using (DataStream packetIn = new(Receive()))
                    {
                        if (packetIn.ReadS32V() != 0x03) throw new ProtocolViolationException();
                    }
                    Client client = new(this, guid, name, properties);
                    OnPlay(client);
                    client.SendRegistries();
                }
                else
                {
                    throw new ProtocolViolationException();
                }
            }
            catch(Exception exception)
            {
                Crashed = true;
                OnDisconnect(exception);
            }
            finally
            {
                if (Crashed) OnDisconnect(null);
            }
        }
        internal byte[] Receive()
        {
            byte[] data = Stream.ReadU8AV();
            if (CompressionThreshold < 0) return data;
            using MemoryStream ms = new(data);
            using DataStream packetIn = new(ms);
            int size = packetIn.ReadS32V();
            int extra = 0;
            for (int value = data.Length; value != 0; value >>= 7) extra++;
            if (size <= 0) return packetIn.ReadU8A(data.Length - extra);
            using DataStream zlib = new(new ZLibStream(ms, CompressionMode.Decompress, false));
            return zlib.ReadU8A(size);
        }
        internal void Send(byte[] data)
        {
            if (CompressionThreshold < 0)
            {
                Stream.WriteU8AV(data);
            }
            else
            {
                if (data.Length < CompressionThreshold)
                {
                    Stream.WriteS32V(data.Length + 1);
                    Stream.WriteU8(0);
                    Stream.WriteU8A(data);
                }
                else
                {
                    using MemoryStream packetStream = new();
                    using (ZLibStream zlib = new(packetStream, CompressionLevel, true)) zlib.Write(data);
                    byte[] compressed = packetStream.ToArray();
                    int extra = 0;
                    for (int value = data.Length; value != 0; value >>= 7) extra++;
                    Stream.WriteS32V(compressed.Length + extra);
                    Stream.WriteS32V(data.Length);
                    Stream.WriteU8A(compressed);
                }
            }
        }

        [GeneratedRegex("^[a-zA-Z0-9_]{1,16}$")]
        private static partial Regex MinecraftUsernameRegex();

        [GeneratedRegex("^[a-zA-Z0-9_]+:[a-zA-Z0-9_]+$")]
        internal static partial Regex NamespaceRegex();
    }
}
