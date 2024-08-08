using Me.Shishioko.Msdl.Connections;
using Me.Shishioko.Msdl.Data;
using Me.Shishioko.Msdl.Data.Chat;
using Me.Shishioko.Msdl.Data.Protocol;
using Net.Myzuc.ShioLib;
using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Numerics;
using System.Security.Cryptography;
using System.Text.Json;
using System.Text;
using System.Threading.Tasks;

namespace Me.Shishioko.Msdl.Clients
{
    public sealed class ClientLogin
    {
        private readonly Client Client;
        public Func<string, Guid, Task> ReceiveStart = (string name, Guid guid) => Task.CompletedTask;
        public Func<string, Guid, Property[], Task> ReceiveAuthentication = (string name, Guid guid, Property[] properties) => Task.CompletedTask;
        public Func<byte[]?, int, Task> ReceiveCustom = (byte[]? data, int sequence) => Task.CompletedTask;
        public Func<string, byte[]?, Task> ReceiveCookieResponse = (string identifier, byte[]? Data) => Task.CompletedTask;
        public Func<ClientConfiguration, Task> SwitchConfiguration = (ClientConfiguration client) => Task.CompletedTask;
        private RSA? EncryptionExchange = null;
        private byte[]? EncryptionVerification = null;
        private bool EncryptionAuthentication = true;
        private string EncryptionServer = string.Empty;
        private string EncryptionName = string.Empty;
        private bool Occupied = false;
        private bool Complete = false;
        internal ClientLogin(Client client)
        {
            Client = client;
        }
        public async Task StartReceivingAsync()
        {
            if (Complete) throw new InvalidOperationException();
            if (Occupied) throw new InvalidOperationException();
            Occupied = true;
            while (true)
            {
                using MemoryStream packetIn = new(await Client.ReceiveAsync());
                switch (packetIn.ReadS32V())
                {
                    case Packets.IncomingLoginStart:
                        {
                            string name = EncryptionName = packetIn.ReadString(SizePrefix.S32V, 16);
                            Guid guid = packetIn.ReadGuid();
                            await ReceiveStart(name, guid);
                            break;
                        }
                    case Packets.IncomingLoginEncryptionResponse:
                        {
                            if (EncryptionExchange is null || EncryptionVerification is null) throw new ProtocolViolationException();
                            byte[] secret = EncryptionExchange.Decrypt(packetIn.ReadU8A(SizePrefix.S32V, 32767), RSAEncryptionPadding.Pkcs1);
                            if (!EncryptionExchange.Decrypt(packetIn.ReadU8A(SizePrefix.S32V, 32767), RSAEncryptionPadding.Pkcs1).SequenceEqual(EncryptionVerification)) throw new ProtocolViolationException();
                            await Client.SyncWrite.WaitAsync();
                            await Client.SyncRead.WaitAsync();
                            try
                            {
                                Client.Stream = new AesCfbStream(Client.Stream, secret, secret, false);
                            }
                            finally
                            {
                                Client.SyncWrite.Release();
                                Client.SyncRead.Release();
                            }
                            if (!EncryptionAuthentication) break;
                            BigInteger number = new(SHA1.HashData(Encoding.ASCII.GetBytes(EncryptionServer).Concat(secret).Concat(EncryptionExchange.ExportSubjectPublicKeyInfo()).ToArray()).Reverse().ToArray());
                            string url = $"https://sessionserver.mojang.com/session/minecraft/hasJoined?username={EncryptionName}&serverId={(number < 0 ? "-" + (-number).ToString("x") : number.ToString("x"))}";
                            using HttpClient http = new();
                            using HttpRequestMessage request = new(HttpMethod.Get, url);
                            using HttpResponseMessage response = await http.SendAsync(request);
                            if (response.StatusCode != HttpStatusCode.OK) throw new UnauthorizedAccessException();
                            using Stream stream = await response.Content.ReadAsStreamAsync();
                            using JsonDocument auth = JsonDocument.Parse(stream);
                            string name = auth.RootElement.GetProperty("name").GetString()!;
                            Guid guid = Guid.ParseExact(auth.RootElement.GetProperty("id").GetString()!, "N");
                            Property[] properties = auth.RootElement.GetProperty("properties")!.EnumerateArray().Select(property => new Property(property.GetProperty("name").GetString()!, property.GetProperty("value").GetString()!, property.GetProperty("signature").GetString())).ToArray();
                            await ReceiveAuthentication(name, guid, properties);
                            break;
                        }
                    case Packets.IncomingLoginPluginResponse:
                        {
                            int sequence = packetIn.ReadS32V();
                            byte[]? data = packetIn.ReadBool() ? packetIn.ReadU8A((int)(packetIn.Length - packetIn.Position)) : null;
                            await ReceiveCustom(data, sequence);
                            break;
                        }
                    case Packets.IncomingLoginEnd:
                        {
                            if (!Complete) throw new ProtocolViolationException();
                            await SwitchConfiguration(new ClientConfiguration(Client));
                            return;
                        }
                    case Packets.IncomingLoginCookieResponse:
                        {
                            string identifier = packetIn.ReadString(SizePrefix.S32V, 32767);
                            byte[]? data = packetIn.ReadBool() ? packetIn.ReadU8A(SizePrefix.S32V, 5120) : null;
                            await ReceiveCookieResponse(identifier, data);
                            break;
                        }
                    default:
                        {
                            throw new ProtocolViolationException();
                        }
                }
            }
        }
        public async Task SendDisconnectAsync(ChatComponent message)
        {
            if (Complete) throw new InvalidOperationException();
            using MemoryStream packetOut = new();
            packetOut.WriteS32V(Packets.OutgoingLoginDisconnect);
            packetOut.WriteString(message.TextSerialize(), SizePrefix.S32V);
            await Client.SendAsync(packetOut.ToArray());
            Client.Dispose();
        }
        public Task SendEncryptionRequestAsync(string server, bool authenticate)
        {
            if (Complete) throw new InvalidOperationException();
            if (EncryptionExchange is not null || EncryptionVerification is not null) throw new InvalidOperationException();
            EncryptionExchange = RSA.Create();
            EncryptionVerification = RandomNumberGenerator.GetBytes(4);
            EncryptionServer = server;
            EncryptionAuthentication = authenticate;
            using MemoryStream packetOut = new();
            packetOut.WriteS32V(Packets.OutgoingLoginEncryptionRequest);
            packetOut.WriteString(server, SizePrefix.S32V);
            packetOut.WriteU8A(EncryptionExchange.ExportSubjectPublicKeyInfo(), SizePrefix.S32V);
            packetOut.WriteU8A(EncryptionVerification, SizePrefix.S32V);
            packetOut.WriteBool(authenticate);
            return Client.SendAsync(packetOut.ToArray());
        }
        public Task SendEndAsync(Guid guid, string name, Property[] properties)
        {
            if (Complete) throw new InvalidOperationException();
            Complete = true;
            using MemoryStream packetOut = new();
            packetOut.WriteS32V(Packets.OutgoingLoginSuccess);
            packetOut.WriteGuid(guid);
            packetOut.WriteString(name, SizePrefix.S32V, 16);
            packetOut.WriteS32V(properties.Length);
            foreach (Property property in properties)
            {
                packetOut.WriteString(property.Name, SizePrefix.S32V, 32767);
                packetOut.WriteString(property.Value, SizePrefix.S32V, 32767);
                packetOut.WriteBool(property.Signature is not null);
                if (property.Signature is not null) packetOut.WriteString(property.Signature, SizePrefix.S32V, 32767);
            }
            packetOut.WriteBool(true);
            return Client.SendAsync(packetOut.ToArray());
        }
        public async Task SendCompressionAsync(int compressionThreshold, CompressionLevel compressionLevel)
        {
            if (Complete) throw new InvalidOperationException();
            using MemoryStream packetOut = new();
            packetOut.WriteS32V(Packets.OutgoingLoginCompression);
            packetOut.WriteS32V(compressionThreshold);
            await Client.SendAsync(packetOut.ToArray());
            Client.CompressionThreshold = compressionThreshold;
            Client.CompressionLevel = compressionLevel;
        }
        public Task SendCustomAsync(string channel, byte[] data, int sequence)
        {
            using MemoryStream packetOut = new();
            packetOut.WriteS32V(Packets.OutgoingLoginPluginRequest);
            packetOut.WriteS32V(sequence);
            packetOut.WriteString(channel, SizePrefix.S32V, 1048576);
            packetOut.WriteU8A(data);
            return Client.SendAsync(packetOut.ToArray());
        }
        public Task SendCookieRequestAsync(string identifier)
        {
            using MemoryStream packetOut = new();
            packetOut.WriteS32V(Packets.OutgoingLoginCookieRequest);
            packetOut.WriteString(identifier, SizePrefix.S32V, 32767);
            return Client.SendAsync(packetOut.ToArray());
        }
    }
}
