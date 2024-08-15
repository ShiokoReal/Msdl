using Net.Myzuc.PurpleStainedGlass.Protocol;
using Net.Myzuc.PurpleStainedGlass.Protocol.Packets;
using Net.Myzuc.PurpleStainedGlass.Protocol.Packets.Login;
using Microsoft.VisualStudio.Threading;
using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Security;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using Net.Myzuc.PurpleStainedGlass.Protocol.Packets.Handshake;
using Net.Myzuc.PurpleStainedGlass.Protocol.Objects.Chat;

namespace Net.Myzuc.PurpleStainedGlass
{
    public sealed class LoginRequest //TODO: callback for configuation possibility
    {
        private readonly Client Client;
        private readonly SemaphoreSlim Sync = new(1, 1);
        private readonly Dictionary<string, AsyncManualResetEvent> CookieSync = [];
        private readonly Dictionary<string, byte[]?> CookieData = [];
        private int CustomSequence = 0;
        private readonly Dictionary<int, AsyncManualResetEvent> CustomSync = [];
        private readonly Dictionary<int, byte[]?> CustomData = [];
        private AsyncManualResetEvent? EncryptionSync = null;
        private ServerboundEncryptionResponse? EncryptionResponse = null;
        private bool Complete = false;
        public readonly ServerboundHandshake Handshake;
        public readonly ServerboundStart Login;
        public Guid Guid;
        public string Name;
        public List<(string name, string value, string? signature)> Properties = [];
        internal LoginRequest(Client client, ServerboundHandshake handshake, ServerboundStart login)
        {
            Client = client;
            Handshake = handshake;
            Login = login;
            Guid = login.Guid;
            Name = login.Name;
        }
        internal async Task StartReceivingAsync()
        {
            while (true)
            {
                Serverbound serverbound = await Client.ReceiveAsync();
                switch (serverbound)
                {
                    case ServerboundEncryptionResponse packet:
                        {
                            if (EncryptionSync is null) throw new ProtocolViolationException();
                            EncryptionResponse = packet;
                            EncryptionSync.Set();
                            EncryptionSync.Reset();
                            await EncryptionSync.WaitAsync();
                            EncryptionSync = null;
                            EncryptionResponse = null;
                            break;
                        }
                    case ServerboundCustomResponse packet:
                        {
                            await Sync.WaitAsync();
                            try
                            {
                                if (!CustomSync.TryGetValue(packet.Sequence, out AsyncManualResetEvent? sync)) throw new ProtocolViolationException();
                                CustomData.Add(packet.Sequence, packet.Data);
                                sync.Set();
                            }
                            finally
                            {
                                Sync.Release();
                            }
                            break;
                        }
                    case ServerboundEnd packet:
                        {
                            if (!Complete) throw new ProtocolViolationException();
                            return;
                        }
                    case ServerboundCookieResponse packet:
                        {
                            await Sync.WaitAsync();
                            try
                            {
                                if (CookieData.ContainsKey(packet.Identifier)) throw new ProtocolViolationException();
                                if (!CookieSync.TryGetValue(packet.Identifier, out AsyncManualResetEvent? sync)) break;
                                CookieData.Add(packet.Identifier, packet.Data);
                                sync.Set();
                            }
                            finally
                            {
                                Sync.Release();
                            }
                            break;
                        }
                }
            }
        }
        internal async Task StopReceivingAsync()
        {
            await Sync.WaitAsync();
            try
            {
                Complete = true;
                Client.Send(new ClientboundEnd(Guid, Name, [..Properties]));
            }
            finally
            {
                Sync.Release();
            }
        }
        public async Task<(string name, string value, string? signature)[]?> EncryptAsync(string server, bool authenticate)
        {
            await Sync.WaitAsync();
            try
            {
                if (Complete) throw new InvalidOperationException();
                using RSA rsa = RSA.Create();
                rsa.KeySize = 1024;
                byte[] verification = RandomNumberGenerator.GetBytes(4);
                EncryptionSync = new();
                Client.Send(new ClientboundEncryptionRequest(server, rsa, verification, authenticate));
                await EncryptionSync.WaitAsync();
                try
                {
                    if (!verification.SequenceEqual(EncryptionResponse!.Verification)) throw new SecurityException();
                    if (!authenticate) return null;
                    (Guid guid, string name, (string name, string value, string? signature)[] properties) identity = await Authentication.AuthenticateAsync(Login.Name, server, EncryptionResponse!.Secret, rsa) ?? throw new SecurityException();
                    Guid = identity.guid;
                    Name = identity.name;
                    return identity.properties;
                }
                finally
                {
                    EncryptionSync.Reset();
                }
            }
            finally
            {
                Sync.Release();
            }
        }
        public async Task SetCompressionAsync(int compressionThreshold, CompressionLevel compressionLevel)
        {
            await Sync.WaitAsync();
            try
            {
                if (Complete) throw new InvalidOperationException();
                Client.Send(new ClientboundCompression(compressionThreshold, compressionLevel));
            }
            finally
            {
                Sync.Release(); 
            }
        }
        public async Task DisconnectAsync(ChatComponent message)
        {
            await Sync.WaitAsync();
            try
            {
                if (Complete) throw new InvalidOperationException();
                Client.Send(new ClientboundDisconnect(message));
            }
            finally
            {
                Sync.Release();
            }
        }
        public async Task<byte[]?> RequestCookieAsync(string identifier)
        {
            AsyncManualResetEvent? sync;
            await Sync.WaitAsync();
            try
            {
                if (Complete) throw new InvalidOperationException();
                if (CookieData.TryGetValue(identifier, out byte[]? data)) return data;
                if (!CookieSync.TryGetValue(identifier, out sync))
                {
                    CookieSync.Add(identifier, sync = new());
                    Client.Send(new ClientboundCookieRequest(identifier));
                }
            }
            finally
            {
                Sync.Release();
            }
            await sync.WaitAsync();
            await Sync.WaitAsync();
            try
            {
                return CookieData[identifier];
            }
            finally
            {
                Sync.Release();
            }
        }
        public async Task<byte[]?> ExchangeCustomAsync(string channel, byte[] data)
        {
            AsyncManualResetEvent? sync = new();
            int sequence;
            await Sync.WaitAsync();
            try
            {
                if (Complete) throw new InvalidOperationException();
                sequence = CustomSequence++;
                CustomSync.Add(sequence, sync = new());
                Client.Send(new ClientboundCustomRequest(sequence, channel, data));
            }
            finally
            {
                Sync.Release();
            }
            await sync.WaitAsync();
            await Sync.WaitAsync();
            try
            {
                return CustomData[sequence];
            }
            finally
            {
                Sync.Release();
            }
        }
    }
}
