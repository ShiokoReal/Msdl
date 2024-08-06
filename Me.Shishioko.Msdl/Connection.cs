using Me.Shishioko.Msdl.Data;
using Net.Myzuc.ShioLib;
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
using System.Text.RegularExpressions;
using System.Diagnostics.Contracts;
using Me.Shishioko.Msdl.Data.Chat;
using System.Collections.Concurrent;
using System.Runtime.InteropServices;
using System.Threading;
using Me.Shishioko.Msdl.Data.Entities;

namespace Me.Shishioko.Msdl
{
    public sealed partial class Connection
    {
        private Stream Stream;
        private SemaphoreSlim Sync = new(1, 1);
        private ProtocolState State = ProtocolState.Handshake;
        private int CompressionThreshold = -1;
        private CompressionLevel CompressionLevel = CompressionLevel.Optimal;
        private bool disconnected = true;

        private ConcurrentQueue<byte[]> PacketQueue = new();
        private Dimension[]? DimensionTypes = null;
        private Dimension? DimensionType = null;
        private string[]? DimensionNames = null;
        private string? DimensionName = null;
        private Biome[]? Biomes = null;
        //TODO: damage types?
        private Gamemode Gamemode = Gamemode.Survival;
        private int EID = 0;
        private bool Hardcore = false;
        private bool RespawnScreen = false;

        private int ContainerSequence = -1;

        public ProtocolState ProtocolState => State;

        public Func<string, byte[], Task> ReceiveConfigurationMessageAsync = (string channel, byte[] data) => Task.CompletedTask;
        public Func<Preferences, Task> ReceivePreferencesAsync = (Preferences preferences) => Task.CompletedTask;
        public Func<long, Task> ReceiveHeartbeatAsync = (long id) => Task.CompletedTask;

        public Func<string, Task> ReceiveChatAsync = (string text) => Task.CompletedTask;
        public Func<string, Task> ReceiveCommandAsync = (string text) => Task.CompletedTask;
        public Func<double, double, double, Task> ReceiveLocationAsync = (double x, double y, double z) => Task.CompletedTask;
        public Func<float, float, Task> ReceiveRotationAsync = (float yaw, float pitch) => Task.CompletedTask;
        public Func<bool, Position, BlockFace, float, float, float, bool, Task> ReceiveInteractionBlockAsync = (bool offhanded, Position position, BlockFace face, float cursorX, float cursorY, float cursorZ, bool inside) => Task.CompletedTask;
        public Func<bool, Task> ReceiveSwingAsync = (bool offhanded) => Task.CompletedTask;
        public Func<PlayerAction, Task> ReceiveActionAsync = (PlayerAction action) => Task.CompletedTask;
        public Func<Position, float, BlockFace, Task> ReceiveBreakAsync = (Position location, float progress, BlockFace face) => Task.CompletedTask;
        public Func<int, Task> ReceiveHotbarAsync = (int slot) => Task.CompletedTask;
        public Connection(Stream stream)
        {
            Stream = stream;
        }
        public async Task DisconnectAsync(ChatComponent? message = null)
        {
            try
            {
                if (message is not null)
                {
                    if (State == ProtocolState.Login)
                    {
                        using MemoryStream packetOut = new();
                        packetOut.WriteS32V(ProtocolPackets.OutgoingLoginDisconnect);
                        packetOut.WriteString(message.TextSerialize(), SizePrefix.S32V, 262144);
                        await SendAsync(packetOut.ToArray());
                    }
                    else if (State == ProtocolState.Configuration || State == ProtocolState.Play)
                    {
                        using MemoryStream packetOut = new();
                        if (State == ProtocolState.Configuration) packetOut.WriteS32V(ProtocolPackets.OutgoingConfigurationDisconnect);
                        else packetOut.WriteS32V(ProtocolPackets.OutgoingPlayDisconnect);
                        packetOut.WriteU8(0x0A);
                        message.Serialize(packetOut);
                        await SendAsync(packetOut.ToArray());
                    }
                }
            }
            finally
            {
                Stream.Close();
            }
        }
        public async Task<(ProtocolState, string, ushort)> ReceiveHandshakeAsync()
        {
            Contract.Assert(State == ProtocolState.Handshake);
            using MemoryStream packetIn = new(await ReceiveAsync());
            if (packetIn.ReadS32V() != ProtocolPackets.IncomingHandshakeHandshake) throw new ProtocolViolationException("Expected handshake packet");
            int version = packetIn.ReadS32V();
            string address = packetIn.ReadString(SizePrefix.S32V);
            ushort port = packetIn.ReadU16();
            int state = packetIn.ReadS32V();
            if (state == 1) State = ProtocolState.Status;
            else if (state == 2) State = ProtocolState.Login;
            else throw new ProtocolViolationException();
            if (State == ProtocolState.Login && version != 766) State = ProtocolState.Disconnected;
            return (State, address, port);
        }
        public async Task<bool> ExchangeStatusStatusAsync(ServerStatus status)
        {
            Contract.Assert(State == ProtocolState.Status);
            using MemoryStream packetIn = new(await ReceiveAsync());
            if (packetIn.ReadS32V() != ProtocolPackets.IncomingStatusStatusRequest) return false;
            using MemoryStream packetOut = new();
            packetOut.WriteS32V(ProtocolPackets.OutgoingStatusStatusResponse);
            packetOut.WriteString(status.TextSerialize(), SizePrefix.S32V);
            await SendAsync(packetOut.ToArray());
            return true;
        }
        public async Task<bool> ExchangeStatusPingAsync()
        {
            Contract.Assert(State == ProtocolState.Status);
            using MemoryStream packetIn = new(await ReceiveAsync());
            if (packetIn.ReadS32V() != ProtocolPackets.IncomingStatusPingRequest) return false;
            using MemoryStream packetOut = new();
            packetOut.WriteS32V(ProtocolPackets.OutgoingStatusPingResponse);
            packetOut.WriteU64(packetIn.ReadU64());
            await SendAsync(packetOut.ToArray());
            return true;
        }
        //TODO: no exchange?
        public async Task<(string, Guid)> ReceiveLoginStartAsync()
        {
            Contract.Assert(State == ProtocolState.Login);
            using MemoryStream packetIn = new(await ReceiveAsync());
            if (packetIn.ReadS32V() != ProtocolPackets.IncomingLoginStart) throw new ProtocolViolationException();
            return (packetIn.ReadString(SizePrefix.S32V), packetIn.ReadGuid());
        }
        public Task SendDisconnectAsync(ChatComponent message)
        {
            Contract.Assert(State == ProtocolState.Login);
            using MemoryStream packetOut = new();
            packetOut.WriteS32V(ProtocolPackets.OutgoingLoginDisconnect);
            packetOut.WriteString(message.TextSerialize(), SizePrefix.S32V);
            return SendAsync(packetOut.ToArray());
        }
        public async Task<Property[]?> ExchangeLoginEncryptionAsync(Guid guid, string name, string server, bool authenticate)
        {
            Contract.Assert(State == ProtocolState.Login);
            Contract.Assert(name.Length <= 16);
            Contract.Assert(server.Length <= 20);
            byte[] verify = RandomNumberGenerator.GetBytes(4);
            using RSA rsa = RSA.Create();
            using (MemoryStream packetOut = new())
            {
                packetOut.WriteS32V(ProtocolPackets.OutgoingLoginEncryptionRequest);
                packetOut.WriteString(server, SizePrefix.S32V);
                packetOut.WriteU8A(rsa.ExportSubjectPublicKeyInfo(), SizePrefix.S32V);
                packetOut.WriteU8A(verify, SizePrefix.S32V);
                packetOut.WriteBool(authenticate);
                await SendAsync(packetOut.ToArray());
            }
            using MemoryStream packetIn = new(await ReceiveAsync());
            if (packetIn.ReadS32V() != ProtocolPackets.IncomingLoginEncryptionResponse) throw new ProtocolViolationException();
            byte[] secret = rsa.Decrypt(packetIn.ReadU8A(SizePrefix.S32V), RSAEncryptionPadding.Pkcs1);
            if (!rsa.Decrypt(packetIn.ReadU8A(SizePrefix.S32V), RSAEncryptionPadding.Pkcs1).SequenceEqual(verify)) throw new ProtocolViolationException();
            Stream = new AesCfbStream(Stream, secret, secret, false);
            if (authenticate)
            {
                BigInteger number = new(SHA1.HashData(Encoding.ASCII.GetBytes(server).Concat(secret).Concat(rsa.ExportSubjectPublicKeyInfo()).ToArray()).Reverse().ToArray());
                string url = $"https://sessionserver.mojang.com/session/minecraft/hasJoined?username={name}&serverId={(number < 0 ? "-" + (-number).ToString("x") : number.ToString("x"))}";
                using HttpClient http = new();
                using HttpRequestMessage request = new(HttpMethod.Get, url);
                using HttpResponseMessage response = await http.SendAsync(request);
                if (response.StatusCode != HttpStatusCode.OK) return null;
                using Stream stream = await response.Content.ReadAsStreamAsync();
                using JsonDocument auth = JsonDocument.Parse(stream);
                if (name != auth.RootElement.GetProperty("name").GetString()!) return null;
                if (!MinecraftUsernameRegex().IsMatch(name)) throw new ProtocolViolationException();
                if (guid != Guid.ParseExact(auth.RootElement.GetProperty("id").GetString()!, "N")) return null;
                return auth.RootElement.GetProperty("properties")!.EnumerateArray().Select(property => new Property(property.GetProperty("name").GetString()!, property.GetProperty("value").GetString()!, property.GetProperty("signature").GetString())).ToArray();
            }
            return [];
        }
        public Task SendLoginCompressionAsync(int compressionThreshold, CompressionLevel compressionLevel)
        {
            Contract.Assert(State == ProtocolState.Login);
            using MemoryStream packetOut = new();
            packetOut.WriteS32V(ProtocolPackets.OutgoingLoginCompression);
            packetOut.WriteS32V(compressionThreshold);
            Task task = SendAsync(packetOut.ToArray());
            CompressionThreshold = compressionThreshold;
            CompressionLevel = compressionLevel;
            return task;
        }
        public async Task<byte[]> ExchangeLoginMessageAsync(string channel, byte[] data)
        {
            Contract.Assert(State == ProtocolState.Login);
            using MemoryStream packetOut = new();
            packetOut.WriteS32V(ProtocolPackets.OutgoingLoginPluginRequest);
            packetOut.WriteS32V(0);
            packetOut.WriteString(channel, SizePrefix.S32V);
            packetOut.WriteU8A(data);
            await SendAsync(packetOut.ToArray());
            using MemoryStream packetIn = new(await ReceiveAsync());
            if (packetIn.ReadS32V() != ProtocolPackets.IncomingLoginPluginResponse) throw new ProtocolViolationException();
            if (packetIn.ReadS32V() != 0) throw new ProtocolViolationException();
            if (packetIn.ReadString(SizePrefix.S32V) != channel) throw new ProtocolViolationException();
            return packetIn.ReadU8A((int)(packetIn.Length - packetIn.Position));
        }
        public async Task ExchangeLoginEndAsync(Guid guid, string name, Property[] properties)
        {
            Contract.Assert(State == ProtocolState.Login);
            Contract.Assert(name.Length <= 16);
            using MemoryStream packetOut = new();
            packetOut.WriteS32V(ProtocolPackets.OutgoingLoginSuccess);
            packetOut.WriteGuid(guid);
            packetOut.WriteString(name, SizePrefix.S32V);
            packetOut.WriteS32V(properties.Length);
            foreach (Property property in properties)
            {
                packetOut.WriteString(property.Name, SizePrefix.S32V);
                packetOut.WriteString(property.Value, SizePrefix.S32V);
                packetOut.WriteBool(property.Signature is not null);
                if (property.Signature is not null) packetOut.WriteString(property.Signature, SizePrefix.S32V);
            }
            packetOut.WriteBool(true);
            await SendAsync(packetOut.ToArray());
            using MemoryStream packetIn =new(await ReceiveAsync());
            if (packetIn.ReadS32V() != ProtocolPackets.IncomingLoginEnd) throw new ProtocolViolationException();
            State = ProtocolState.Configuration;
            _ = ListenAsync();
        }
        public async Task ProcessConfigurationAsync()
        {
            Contract.Assert(State == ProtocolState.Configuration);
            if (disconnected)
            {
                State = ProtocolState.Disconnected;
                return;
            }
            while (true)
            {
                if (!PacketQueue.TryDequeue(out byte[]? packetDataIn)) break;
                using MemoryStream packetIn = new(packetDataIn);
                switch (packetIn.ReadS32V())
                {
                    case ProtocolPackets.IncomingConfigurationInformation:
                        {
                            string language = packetIn.ReadString(SizePrefix.S32V, 16);
                            byte renderDistance = packetIn.ReadU8();
                            ChatMode chatMode = (ChatMode)packetIn.ReadS32V();
                            bool chatColors = packetIn.ReadBool();
                            SkinMask skinMask = new(packetIn.ReadU8());
                            bool rightHanded = packetIn.ReadS32V() == 0 ? false : true;
                            packetIn.ReadBool();
                            bool listing = packetIn.ReadBool();
                            await ReceivePreferencesAsync(new(language, renderDistance, chatMode, chatColors, skinMask, rightHanded, listing));
                            break;
                        }
                    case ProtocolPackets.IncomingConfigurationCookieResponse:
                        {
                            //TODO:
                            break;
                        }
                    case ProtocolPackets.IncomingConfigurationPluginMessage:
                        {
                            string channel = packetIn.ReadString(SizePrefix.S32V);
                            if (!Connection.NamespaceRegex().IsMatch(channel)) throw new ProtocolViolationException();
                            byte[] data = packetIn.ReadU8A((int)(packetIn.Length - packetIn.Position));
                            await ReceiveConfigurationMessageAsync(channel, data);
                            break;
                        }
                    case ProtocolPackets.IncomingConfigurationEnd:
                        {
                            State = ProtocolState.Play; //TODO: check if expected
                            return;
                        }
                    case ProtocolPackets.IncomingConfigurationHeartbeat:
                        {
                            await ReceiveHeartbeatAsync(packetIn.ReadS64());
                            break;
                        }
                    default:
                        {
                            throw new ProtocolViolationException();
                        }
                }
            }
        }
        public Task SendConfigurationRegistryAsync(Dimension[] dimensionTypes)
        {
            Contract.Assert(State == ProtocolState.Configuration);
            Contract.Assert(dimensionTypes.Length > 0);
            DimensionTypes = dimensionTypes;
            using MemoryStream packetOut = new();
            packetOut.WriteS32V(ProtocolPackets.OutgoingConfigurationRegistry);
            packetOut.WriteString("minecraft:dimension_type", SizePrefix.S32V);
            packetOut.WriteS32V(DimensionTypes.Length);
            for (int i = 0; i < DimensionTypes.Length; i++)
            {
                Dimension dimensionType = DimensionTypes[i];
                packetOut.WriteString(dimensionType.Name, SizePrefix.S32V);
                packetOut.WriteBool(true);
                packetOut.WriteU8(0x0A);
                packetOut.WriteU8(5);
                packetOut.WriteString("ambient_light", SizePrefix.S16);
                packetOut.WriteF32(dimensionType.AmbientLight);
                packetOut.WriteU8(1);
                packetOut.WriteString("bed_works", SizePrefix.S16);
                packetOut.WriteBool(true);
                packetOut.WriteU8(6);
                packetOut.WriteString("coordinate_scale", SizePrefix.S16);
                packetOut.WriteF64(1.0);
                packetOut.WriteU8(8);
                packetOut.WriteString("effects", SizePrefix.S16);
                packetOut.WriteString(
                    dimensionType.Effects == Sky.Overworld ? "minecraft:overworld" : //TODO: no enum
                    dimensionType.Effects == Sky.Nether ? "minecraft:the_nether" :
                    dimensionType.Effects == Sky.End ? "minecraft:the_end" :
                    "minecraft:overworld", SizePrefix.S16
                    );
                packetOut.WriteU8(1);
                packetOut.WriteString("has_ceiling", SizePrefix.S16);
                packetOut.WriteBool(false);
                packetOut.WriteU8(1);
                packetOut.WriteString("has_raids", SizePrefix.S16);
                packetOut.WriteBool(true);
                packetOut.WriteU8(1);
                packetOut.WriteString("has_skylight", SizePrefix.S16);
                packetOut.WriteBool(dimensionType.HasSkylight);
                packetOut.WriteU8(3);
                packetOut.WriteString("height", SizePrefix.S16);
                packetOut.WriteS32(dimensionType.Height);
                packetOut.WriteU8(8);
                packetOut.WriteString("infiniburn", SizePrefix.S16);
                packetOut.WriteString("#minecraft:infiniburn_overworld", SizePrefix.S16);
                packetOut.WriteU8(3);
                packetOut.WriteString("logical_height", SizePrefix.S16);
                packetOut.WriteS32(dimensionType.Height);
                packetOut.WriteU8(3);
                packetOut.WriteString("min_y", SizePrefix.S16);
                packetOut.WriteS32(dimensionType.Depth);
                packetOut.WriteU8(3);
                packetOut.WriteString("monster_spawn_block_light_limit", SizePrefix.S16);
                packetOut.WriteS32(0);
                packetOut.WriteU8(3);
                packetOut.WriteString("monster_spawn_light_level", SizePrefix.S16);
                packetOut.WriteS32(0);
                packetOut.WriteU8(1);
                packetOut.WriteString("natural", SizePrefix.S16);
                packetOut.WriteBool(dimensionType.Natural);
                packetOut.WriteU8(1);
                packetOut.WriteString("piglin_safe", SizePrefix.S16);
                packetOut.WriteBool(false);
                packetOut.WriteU8(1);
                packetOut.WriteString("respawn_anchor_works", SizePrefix.S16);
                packetOut.WriteBool(true);
                packetOut.WriteU8(1);
                packetOut.WriteString("ultrawarm", SizePrefix.S16);
                packetOut.WriteBool(false);
                packetOut.WriteU8(0);
            }
            return SendAsync(packetOut.ToArray());
        }
        public Task SendConfigurationRegistryAsync(Biome[] biomes)
        {
            Contract.Assert(State == ProtocolState.Configuration);
            Contract.Assert(biomes.Length > 0);
            Biomes = biomes;
            using MemoryStream packetOut = new();
            packetOut.WriteS32V(ProtocolPackets.OutgoingConfigurationRegistry);
            packetOut.WriteString("minecraft:worldgen/biome", SizePrefix.S32V);
            packetOut.WriteS32V(Biomes.Length);
            for(int i = 0; i < Biomes.Length; i++)
            {
                Biome biome = Biomes[i];
                packetOut.WriteString(biome.Name, SizePrefix.S32V);
                packetOut.WriteBool(true);
                packetOut.WriteU8(0x0A);
                packetOut.WriteU8(1);
                packetOut.WriteString("has_precipitation", SizePrefix.S16);
                packetOut.WriteBool(biome.Precipitation);
                packetOut.WriteU8(5);
                packetOut.WriteString("temperature", SizePrefix.S16);
                packetOut.WriteF32(0.0f);
                packetOut.WriteU8(5);
                packetOut.WriteString("downfall", SizePrefix.S16);
                packetOut.WriteF32(0.0f);
                packetOut.WriteU8(0x0A);
                packetOut.WriteString("effects", SizePrefix.S16);
                packetOut.WriteU8(3);
                packetOut.WriteString("sky_color", SizePrefix.S16);
                packetOut.WriteU8(biome.SkyColor.A);
                packetOut.WriteU8(biome.SkyColor.R);
                packetOut.WriteU8(biome.SkyColor.G);
                packetOut.WriteU8(biome.SkyColor.B);
                packetOut.WriteU8(3);
                packetOut.WriteString("water_fog_color", SizePrefix.S16);
                packetOut.WriteU8(biome.WaterFogColor.A);
                packetOut.WriteU8(biome.WaterFogColor.R);
                packetOut.WriteU8(biome.WaterFogColor.G);
                packetOut.WriteU8(biome.WaterFogColor.B);
                packetOut.WriteU8(3);
                packetOut.WriteString("fog_color", SizePrefix.S16);
                packetOut.WriteU8(biome.FogColor.A);
                packetOut.WriteU8(biome.FogColor.R);
                packetOut.WriteU8(biome.FogColor.G);
                packetOut.WriteU8(biome.FogColor.B);
                packetOut.WriteU8(3);
                packetOut.WriteString("water_color", SizePrefix.S16);
                packetOut.WriteU8(biome.WaterColor.A);
                packetOut.WriteU8(biome.WaterColor.R);
                packetOut.WriteU8(biome.WaterColor.G);
                packetOut.WriteU8(biome.WaterColor.B);
                if (biome.FoliageColor.HasValue)
                {
                    packetOut.WriteU8(3);
                    packetOut.WriteString("foliage_color", SizePrefix.S16);
                    packetOut.WriteU8(biome.FoliageColor.Value.A);
                    packetOut.WriteU8(biome.FoliageColor.Value.R);
                    packetOut.WriteU8(biome.FoliageColor.Value.G);
                    packetOut.WriteU8(biome.FoliageColor.Value.B);
                }
                if (biome.GrassColor.HasValue)
                {
                    packetOut.WriteU8(3);
                    packetOut.WriteString("grass_color", SizePrefix.S16);
                    packetOut.WriteU8(biome.GrassColor.Value.A);
                    packetOut.WriteU8(biome.GrassColor.Value.R);
                    packetOut.WriteU8(biome.GrassColor.Value.G);
                    packetOut.WriteU8(biome.GrassColor.Value.B);
                }
                if (biome.Music.HasValue)
                {
                    packetOut.WriteU8(0x0A);
                    packetOut.WriteString("music", SizePrefix.S16);
                    packetOut.WriteU8(1);
                    packetOut.WriteString("replace_current_music", SizePrefix.S16);
                    packetOut.WriteBool(biome.Music.Value.ReplaceCurrentMusic);
                    packetOut.WriteU8(8);
                    packetOut.WriteString("sound", SizePrefix.S16);
                    packetOut.WriteString(biome.Music.Value.Sound, SizePrefix.S16);
                    packetOut.WriteU8(3);
                    packetOut.WriteString("max_delay", SizePrefix.S16);
                    packetOut.WriteS32((int)(biome.Music.Value.MaxDelay.TotalSeconds * 20.0d));
                    packetOut.WriteU8(3);
                    packetOut.WriteString("min_delay", SizePrefix.S16);
                    packetOut.WriteS32((int)(biome.Music.Value.MinDelay.TotalSeconds * 20.0d));
                    packetOut.WriteU8(0);
                }
                if (biome.AmbientSound is not null)
                {
                    packetOut.WriteU8(8);
                    packetOut.WriteString("ambient_sound", SizePrefix.S16);
                    packetOut.WriteString(biome.AmbientSound, SizePrefix.S16);
                }
                if (biome.AdditionsSound.HasValue)
                {
                    packetOut.WriteU8(0x0A);
                    packetOut.WriteString("additions_sound", SizePrefix.S16);
                    packetOut.WriteU8(8);
                    packetOut.WriteString("sound", SizePrefix.S16);
                    packetOut.WriteString(biome.AdditionsSound.Value.Sound, SizePrefix.S16);
                    packetOut.WriteU8(6);
                    packetOut.WriteString("tick_chance", SizePrefix.S16);
                    packetOut.WriteF64(biome.AdditionsSound.Value.Chance / 20.0d);
                    packetOut.WriteU8(0);
                }
                if (biome.MoodSound.HasValue)
                {
                    packetOut.WriteU8(0x0A);
                    packetOut.WriteString("additions_sound", SizePrefix.S16);
                    packetOut.WriteU8(8);
                    packetOut.WriteString("sound", SizePrefix.S16);
                    packetOut.WriteString(biome.MoodSound.Value.Sound, SizePrefix.S16);
                    packetOut.WriteU8(3);
                    packetOut.WriteString("tick_delay", SizePrefix.S16);
                    packetOut.WriteS32((int)(biome.MoodSound.Value.Delay.TotalSeconds * 20.0d));
                    packetOut.WriteU8(6);
                    packetOut.WriteString("offset", SizePrefix.S16);
                    packetOut.WriteF64(biome.MoodSound.Value.Offset);
                    packetOut.WriteU8(3);
                    packetOut.WriteString("block_search_extent", SizePrefix.S16);
                    packetOut.WriteS32(biome.MoodSound.Value.BlockSearchExtent);
                    packetOut.WriteU8(0);
                }
                //TODO: particle
                packetOut.WriteU8(0);
                packetOut.WriteU8(0);
            }
            return SendAsync(packetOut.ToArray());
        }
        public async Task SendConfigurationRegistryAsync_Temp_A()
        {
            Contract.Assert(State == ProtocolState.Configuration);
            {
                using MemoryStream packetOut = new();
                packetOut.WriteS32V(ProtocolPackets.OutgoingConfigurationRegistry);
                packetOut.WriteString("minecraft:trim_pattern", SizePrefix.S32V);
                packetOut.WriteS32V(0);
                await SendAsync(packetOut.ToArray());
            }
            {
                using MemoryStream packetOut = new();
                packetOut.WriteS32V(ProtocolPackets.OutgoingConfigurationRegistry);
                packetOut.WriteString("minecraft:trim_material", SizePrefix.S32V);
                packetOut.WriteS32V(0);
                await SendAsync(packetOut.ToArray());
            }
            {
                using MemoryStream packetOut = new();
                packetOut.WriteS32V(ProtocolPackets.OutgoingConfigurationRegistry);
                packetOut.WriteString("minecraft:chat_type", SizePrefix.S32V);
                packetOut.WriteS32V(0);
                await SendAsync(packetOut.ToArray());
            }
            {
                using MemoryStream packetOut = new();
                packetOut.WriteS32V(ProtocolPackets.OutgoingConfigurationRegistry);
                packetOut.WriteString("minecraft:worldgen/biome", SizePrefix.S32V);
                packetOut.WriteS32V(0);
                await SendAsync(packetOut.ToArray());
            }
            {
                using MemoryStream packetOut = new();
                packetOut.WriteS32V(ProtocolPackets.OutgoingConfigurationRegistry);
                packetOut.WriteString("minecraft:wolf_variant", SizePrefix.S32V);
                packetOut.WriteS32V(0);
                await SendAsync(packetOut.ToArray());
            }
            {
                using MemoryStream packetOut = new();
                packetOut.WriteS32V(ProtocolPackets.OutgoingConfigurationRegistry);
                packetOut.WriteString("minecraft:banner_pattern", SizePrefix.S32V);
                packetOut.WriteS32V(0);
                await SendAsync(packetOut.ToArray());
            }
        }
        public Task SendConfigurationRegistryAsync_Temp_B() //TODO: other registries
        {
            Contract.Assert(State == ProtocolState.Configuration);
            using MemoryStream packetOut = new();
            packetOut.WriteS32V(ProtocolPackets.OutgoingConfigurationRegistry);
            packetOut.WriteString("minecraft:damage_type", SizePrefix.S32V);
            string[] damages = [
            "minecraft:generic_kill",
            "minecraft:dragon_breath",
            "minecraft:outside_border",
            "minecraft:freeze",
            "minecraft:stalagmite",
            "minecraft:in_fire",
            "minecraft:wither",
            "minecraft:generic",
            "minecraft:cactus",
            "minecraft:cramming",
            "minecraft:drown",
            "minecraft:fall",
            "minecraft:falling_anvil",
            "minecraft:fireworks",
            "minecraft:in_wall",
            "minecraft:indirect_magic",
            "minecraft:lava",
            "minecraft:lightning_bolt",
            "minecraft:magic",
            "minecraft:mob_attack",
            "minecraft:mob_attack_no_aggro",
            "minecraft:on_fire",
            "minecraft:out_of_world",
            "minecraft:player_explosion",
            "minecraft:starve",
            "minecraft:string",
            "minecraft:sweet_berry_bush",
            "minecraft:dry_out",
            "minecraft:hot_floor",
            "minecraft:fly_into_wall",
            "minecraft:player_attack", //TODO: all vanilla types
            ];
            packetOut.WriteS32V(damages.Length);
            for (int i = 0; i < damages.Length; i++)
            {
                packetOut.WriteString(damages[i], SizePrefix.S32V);
                packetOut.WriteBool(true);
                packetOut.WriteU8(0x0A);
                packetOut.WriteU8(5);
                packetOut.WriteString("exhaustion", SizePrefix.S16);
                packetOut.WriteF32(0.0f);
                packetOut.WriteU8(8);
                packetOut.WriteString("scaling", SizePrefix.S16);
                packetOut.WriteString("always", SizePrefix.S16);
                packetOut.WriteU8(8);
                packetOut.WriteString("message_id", SizePrefix.S16);
                packetOut.WriteString("arrow", SizePrefix.S16);
                packetOut.WriteU8(0);
            }
            return SendAsync(packetOut.ToArray());
        }
        public Task SendFluidsAsync(int[] water, int[] lava)
        {
            Contract.Assert(State == ProtocolState.Configuration || State == ProtocolState.Play);
            using MemoryStream packetOut = new();
            packetOut.WriteS32V(State == ProtocolState.Configuration ? ProtocolPackets.OutgoingConfigurationTags : ProtocolPackets.OutgoingPlayTags);
            packetOut.WriteS32V(1);
            packetOut.WriteString("minecraft:fluid", SizePrefix.S32V);
            packetOut.WriteS32V(2);
            packetOut.WriteString("water", SizePrefix.S32V);
            packetOut.WriteS32V(water.Length);
            for (int i = 0; i < water.Length; i++)
            {
                packetOut.WriteS32V(water[i]);
            }
            packetOut.WriteString("lava", SizePrefix.S32V);
            packetOut.WriteS32V(lava.Length);
            for (int i = 0; i < lava.Length; i++)
            {
                packetOut.WriteS32V(lava[i]);
            }
            return SendAsync(packetOut.ToArray());
        }
        public Task SendConfigurationEndAsync()
        {
            Contract.Assert(State == ProtocolState.Configuration);
            using MemoryStream packetOut = new();
            packetOut.WriteS32V(ProtocolPackets.OutgoingConfigurationEnd);
            return SendAsync(packetOut.ToArray());
        }
        public async Task ProcessPlayAsync()
        {
            Contract.Assert(State == ProtocolState.Play || State == ProtocolState.PlayToConfiguration);
            if (disconnected)
            {
                State = ProtocolState.Disconnected;
                return;
            }
            while (true)
            {
                if (!PacketQueue.TryDequeue(out byte[]? packetDataIn)) break;
                using MemoryStream packetIn = new(packetDataIn);
                if (State == ProtocolState.PlayToConfiguration)
                {
                    if (packetIn.ReadS32V() == ProtocolPackets.IncomingPlayConfigure)
                    {
                        State = ProtocolState.Configuration;
                        return;
                    }
                    continue;
                }
                switch (packetIn.ReadS32V())
                {
                    case ProtocolPackets.IncomingPlayChat:
                        {
                            await ReceiveChatAsync(packetIn.ReadString(SizePrefix.S32V, 256));
                            break;
                        }
                    case ProtocolPackets.IncomingPlayCommand:
                        {
                            await ReceiveCommandAsync(packetIn.ReadString(SizePrefix.S32V, 256));
                            break;
                        }
                    case ProtocolPackets.IncomingPlayHeartbeat:
                        {
                            await ReceiveHeartbeatAsync(packetIn.ReadS64());
                            break;
                        }
                    case ProtocolPackets.IncomingPlayInteractionBlock:
                        {
                            bool offhanded = packetIn.ReadS32V() == 0 ? false : true;
                            Position position = new(packetIn.ReadU64());
                            BlockFace face = (BlockFace)packetIn.ReadS32V();
                            float cursorX = packetIn.ReadF32();
                            float cursorY = packetIn.ReadF32();
                            float cursorZ = packetIn.ReadF32();
                            bool inside = packetIn.ReadBool();
                            using MemoryStream packetOut = new();
                            packetOut.WriteS32V(ProtocolPackets.OutgoingPlayBlockFeedback);
                            packetOut.WriteS32V(packetIn.ReadS32V());
                            await SendAsync(packetOut.ToArray());
                            await ReceiveInteractionBlockAsync(offhanded, position, face, cursorX, cursorY, cursorZ, inside);
                            break;
                        }
                    case ProtocolPackets.IncomingPlayActionGeneric:
                        {
                            int type = packetIn.ReadS32V();
                            Position position = new(packetIn.ReadU64());
                            BlockFace face = (BlockFace)packetIn.ReadS8();
                            using MemoryStream packetOut = new();
                            packetOut.WriteS32V(ProtocolPackets.OutgoingPlayBlockFeedback);
                            packetOut.WriteS32V(packetIn.ReadS32V());
                            await SendAsync(packetOut.ToArray());
                            switch (type)
                            {
                                case 0:
                                    {
                                        await ReceiveBreakAsync(position, Gamemode == Gamemode.Creative ? 1.0f : 0.0f, face);
                                        break;
                                    }
                                case 1:
                                    {
                                        await ReceiveBreakAsync(position, -1.0f, face);
                                        break;
                                    }
                                case 2:
                                    {
                                        await ReceiveBreakAsync(position, Gamemode == Gamemode.Spectator ? 0.0f : 1.0f, face);
                                        break;
                                    }
                                case 3:
                                    {
                                        await ReceiveActionAsync(PlayerAction.DropStack);
                                        break;
                                    }
                                case 4:
                                    {
                                        await ReceiveActionAsync(PlayerAction.DropSingle);
                                        break;
                                    }
                                case 5:
                                    {
                                        //TODO:
                                        break;
                                    }
                                case 6:
                                    {
                                        await ReceiveActionAsync(PlayerAction.Swap);
                                        break;
                                    }
                            }
                            break;
                        }
                    case ProtocolPackets.IncomingPlayActionMovement:
                        {
                            if (packetIn.ReadS32V() != EID) throw new ProtocolViolationException();
                            int type = packetIn.ReadS32V();
                            int strength = packetIn.ReadS32V();
                            switch (type)
                            {
                                case 0:
                                    {
                                        await ReceiveActionAsync(PlayerAction.SneakStart);
                                        break;
                                    }
                                case 1:
                                    {
                                        await ReceiveActionAsync(PlayerAction.SneakStop);
                                        break;
                                    }
                                case 2:
                                    {
                                        //TODO:
                                        break;
                                    }
                                case 3:
                                    {
                                        await ReceiveActionAsync(PlayerAction.SprintStart);
                                        break;
                                    }
                                case 4:
                                    {
                                        await ReceiveActionAsync(PlayerAction.SprintStop);
                                        break;
                                    }
                                case 5:
                                    {
                                        //TODO:
                                        break;
                                    }
                                case 6:
                                    {
                                        //TODO:
                                        break;
                                    }
                                case 7:
                                    {
                                        //TODO:
                                        break;
                                    }
                                case 8:
                                    {
                                        //TODO:
                                        break;
                                    }
                            }
                            break;
                        }
                    case ProtocolPackets.IncomingPlayHotbar:
                        {
                            int slot = packetIn.ReadS16();
                            if (slot < 0 || slot >= 9) throw new ProtocolViolationException();
                            await ReceiveHotbarAsync(slot);
                            break;
                        }
                    case ProtocolPackets.IncomingPlayLocation:
                        {
                            await ReceiveLocationAsync(packetIn.ReadF64(), packetIn.ReadF64(), packetIn.ReadF64());
                            break;
                        }
                    case ProtocolPackets.IncomingPlayPosition:
                        {
                            await ReceiveLocationAsync(packetIn.ReadF64(), packetIn.ReadF64(), packetIn.ReadF64());
                            await ReceiveRotationAsync(packetIn.ReadF32(), packetIn.ReadF32());
                            break;
                        }
                    case ProtocolPackets.IncomingPlayRotation:
                        {
                            await ReceiveRotationAsync(packetIn.ReadF32(), packetIn.ReadF32());
                            break;
                        }
                    case ProtocolPackets.IncomingPlaySwing:
                        {
                            await ReceiveSwingAsync(packetIn.ReadS32V() == 0 ? false : true);
                            break;
                        }
                    case ProtocolPackets.IncomingPlayConfigure:
                        {
                            throw new ProtocolViolationException();
                        }
                    default:
                        {
                            //throw new ProtocolViolationException(); //TODO: remove comment
                            break;
                        }
                }
            }
        }
        public Task SendReconfigureAsync()
        {
            Contract.Assert(State == ProtocolState.Play);
            using MemoryStream packetOut = new();
            packetOut.WriteS32V(ProtocolPackets.OutgoingPlayConfigure);
            State = ProtocolState.PlayToConfiguration;
            return SendAsync(packetOut.ToArray());
        }
        public Task SendHeartbeatAsync(long id)
        {
            Contract.Assert(State == ProtocolState.Play || State == ProtocolState.Configuration);
            using MemoryStream packetOut = new();
            packetOut.WriteS32V(State == ProtocolState.Play ? ProtocolPackets.OutgoingPlayHeartbeat : ProtocolPackets.OutgoingConfigurationHeartbeat);
            packetOut.WriteS64(id);
            return SendAsync(packetOut.ToArray());
        }
        public Task SendTablistUpdate(Guid[] id, (string name, Property[] properties)[]? additions, Gamemode[]? gamemode = null, bool[]? visibility = null, int[]? latency = null, ChatComponent?[]? display = null)
        {
            Contract.Assert(State == ProtocolState.Play);
            if (additions is not null) Contract.Assert(additions.Length == id.Length);
            if (gamemode is not null) Contract.Assert(gamemode.Length == id.Length);
            if (visibility is not null) Contract.Assert(visibility.Length == id.Length);
            if (latency is not null) Contract.Assert(latency.Length == id.Length);
            if (display is not null) Contract.Assert(display.Length == id.Length);
            using MemoryStream packetOut = new();
            packetOut.WriteS32V(ProtocolPackets.OutgoingPlayTablistAction);
            packetOut.WriteU8((byte)((additions is not null ? 0x01 : 0x00) | (gamemode is not null ? 0x04 : 0x00) | (visibility is not null ? 0x08 : 0x00) | (latency is not null ? 0x10 : 0x00) | (display is not null ? 0x20 : 0x00)));
            packetOut.WriteS32V(id.Length);
            for (int i = 0; i < id.Length; i++)
            {
                packetOut.WriteGuid(id[i]);
                if (additions is not null)
                {
                    (string name, Property[] properties) = additions[i];
                    packetOut.WriteString(name, SizePrefix.S32V, 16);
                    packetOut.WriteS32V(properties.Length);
                    for (int e = 0; e < properties.Length; e++)
                    {
                        Property property = properties[e];
                        packetOut.WriteString(property.Name, SizePrefix.S32V, 32767);
                        packetOut.WriteString(property.Value, SizePrefix.S32V, 32767);
                        packetOut.WriteBool(property.Signature is not null);
                        if (property.Signature is not null) packetOut.WriteString(property.Signature, SizePrefix.S32V, 32767);
                    }
                }
                if (gamemode is not null) packetOut.WriteS32V((int)gamemode[i]);
                if (visibility is not null) packetOut.WriteBool(visibility[i]);
                if (latency is not null) packetOut.WriteS32V(latency[i]);
                if (display is not null)
                {
                    ChatComponent? displayName = display[i];
                    packetOut.WriteBool(displayName is not null);
                    if (displayName is not null)
                    {
                        packetOut.WriteU8(0x0A);
                        displayName.Serialize(packetOut);
                    }
                }
            }
            return SendAsync(packetOut.ToArray());
        }
        public Task SendTablistRemove(Guid[] id)
        {
            Contract.Assert(State == ProtocolState.Play);
            using MemoryStream packetOut = new();
            packetOut.WriteS32V(ProtocolPackets.OutgoingPlayTablistRemove);
            packetOut.WriteS32V(id.Length);
            for (int i = 0; i < id.Length; i++)
            {
                packetOut.WriteGuid(id[i]);
            }
            return SendAsync(packetOut.ToArray());
        }
        public Task SendTablistText(ChatComponent header, ChatComponent footer)
        {
            Contract.Assert(State == ProtocolState.Play);
            using MemoryStream packetOut = new();
            packetOut.WriteS32V(ProtocolPackets.OutgoingPlayTablistText);
            packetOut.WriteU8(0x0A);
            header.Serialize(packetOut);
            packetOut.WriteU8(0x0A);
            footer.Serialize(packetOut);
            return SendAsync(packetOut.ToArray());
        }
        public Task SendChatSystemAsync(ChatComponent message)
        {
            Contract.Assert(State == ProtocolState.Play);
            using MemoryStream packetOut = new();
            packetOut.WriteS32V(ProtocolPackets.OutgoingPlayChatSystem);
            packetOut.WriteU8(0x0A);
            message.Serialize(packetOut);
            packetOut.WriteBool(false);
            return SendAsync(packetOut.ToArray());
        }
        public Task SendInitializeAsync(int entityID, bool hardcore, int viewDistance, int simulationDistance, bool reducedDebug, bool respawnScreen, string[] dimensionNames, Dimension dimensionType, string dimensionName, ulong seedHash, Gamemode currentGamemode, Gamemode? previousGamemode, bool flatWorld, DeathLocation? deathLocation)
        {
            Contract.Assert(State == ProtocolState.Play);
            Contract.Assert(DimensionTypes is not null);
            Contract.Assert(dimensionNames is not null);
            Contract.Assert(viewDistance >= 0);
            Contract.Assert(simulationDistance >= 0);
            Contract.Assert(DimensionTypes!.Contains(dimensionType));
            Contract.Assert(dimensionNames!.Contains(dimensionName));
            DimensionNames = dimensionNames;
            using MemoryStream packetOut = new();
            packetOut.WriteS32V(ProtocolPackets.OutgoingPlayStart);
            packetOut.WriteS32(EID = entityID);
            packetOut.WriteBool(Hardcore = hardcore);
            packetOut.WriteS32V(DimensionNames!.Length);
            for (int i = 0; i < DimensionNames.Length; i++) packetOut.WriteString(DimensionNames[i], SizePrefix.S32V);
            packetOut.WriteS32V(0);
            packetOut.WriteS32V(viewDistance);
            packetOut.WriteS32V(simulationDistance);
            packetOut.WriteBool(reducedDebug);
            packetOut.WriteBool(RespawnScreen = respawnScreen);
            packetOut.WriteBool(false);
            packetOut.WriteS32V(Array.IndexOf(DimensionTypes!, DimensionType = dimensionType));
            packetOut.WriteString(DimensionName = dimensionName, SizePrefix.S32V);
            packetOut.WriteU64(seedHash);
            packetOut.WriteU8((byte)(Gamemode = currentGamemode));
            packetOut.WriteS8(previousGamemode.HasValue ? (sbyte)previousGamemode.Value : (sbyte)-1);
            packetOut.WriteBool(false);
            packetOut.WriteBool(flatWorld);
            packetOut.WriteBool(deathLocation.HasValue);
            if (deathLocation.HasValue)
            {
                Contract.Assert(DimensionNames.Contains(deathLocation.Value.Dimension));
                packetOut.WriteString(deathLocation.Value.Dimension, SizePrefix.S32V);
                packetOut.WriteU64(deathLocation.Value.Location.Data);
            }
            packetOut.WriteS32V(0);
            packetOut.WriteBool(false);
            return SendAsync(packetOut.ToArray());
        }
        public Task SendTime(long time)
        {
            Contract.Assert(State == ProtocolState.Play);
            Contract.Assert(DimensionName is not null);
            using MemoryStream packetOut = new();
            packetOut.WriteS32V(ProtocolPackets.OutgoingPlayTime);
            packetOut.WriteS64(time);
            packetOut.WriteS64(time);
            return SendAsync(packetOut.ToArray());
        }
        public Task SendChunkBiomesAsync(int x, int z, int[][] biomes)
        {
            Contract.Assert(State == ProtocolState.Play);
            Contract.Assert(DimensionType.HasValue);
            Contract.Assert(DimensionName is not null);
            Contract.Assert(Biomes is not null);
            Contract.Assert(x < 2097152);
            Contract.Assert(z < 2097152);
            Contract.Assert(x >= -2097152);
            Contract.Assert(z >= -2097152);
            Contract.Assert(biomes.Length * 16 == DimensionType!.Value.Height);
            using MemoryStream packetOut = new();
            packetOut.WriteS32V(ProtocolPackets.OutgoingPlayChunkBiomes);
            packetOut.WriteS32V(1);
            packetOut.WriteS32(x);
            packetOut.WriteS32(z);
            using MemoryStream dataOut = new();
            for (int i = 0; i < biomes.Length; i++)
            {
                int[] biome = biomes[i];
                Contract.Assert(biome.Length == 64);
                Dictionary<int, int> counts = [];
                for (int e = 0; e < 64; e++)
                {
                    int id = Math.Clamp(biome[e], 0, Biomes!.Length);
                    counts.TryAdd(id, 0);
                    counts[id]++;
                }
                byte bits = (byte)Math.Ceiling(Math.Log2(counts.Count));
                byte totalbits = (byte)Math.Ceiling(Math.Log2(Biomes!.Length));
                if (bits >= 4)
                {
                    dataOut.WriteU8(totalbits);
                    SemiCompactArray data = new(totalbits, 64);
                    for (int e = 0; e < data.Length; e++) data[i] = Math.Clamp(biome[e], 0, Biomes.Length);
                    dataOut.WriteU64A(data.Data, SizePrefix.S32V);
                }
                else
                if (bits > 0)
                {
                    dataOut.WriteU8(bits);
                    SemiCompactArray data = new(bits, 64);
                    Dictionary<int, int> mappings = [];
                    dataOut.WriteS32V(counts.Count);
                    foreach (int id in counts.Keys)
                    {
                        mappings[id] = mappings.Count;
                        dataOut.WriteS32V(id);
                    }
                    for (int e = 0; e < data.Length; e++) data[i] = mappings[Math.Clamp(biome[e], 0, Biomes.Length)];
                    dataOut.WriteU64A(data.Data, SizePrefix.S32V);
                }
                else
                {
                    dataOut.WriteU8(0);
                    dataOut.WriteS32V(counts.First().Key);
                    dataOut.WriteS32V(0);
                }
            }
            packetOut.WriteU8A(dataOut.ToArray(), SizePrefix.S32V);
            return SendAsync(packetOut.ToArray());
        }
        public Task SendChunkLightAsync(int x, int z, SemiCompactArray?[] skyLight, SemiCompactArray?[] blockLight)
        {
            Contract.Assert(State == ProtocolState.Play);
            Contract.Assert(DimensionType.HasValue);
            Contract.Assert(DimensionName is not null);
            Contract.Assert(x < 2097152);
            Contract.Assert(z < 2097152);
            Contract.Assert(x >= -2097152);
            Contract.Assert(z >= -2097152);
            Contract.Assert(blockLight.Length * 16 == DimensionType!.Value.Height + 2);
            Contract.Assert(skyLight.Length * 16 == DimensionType!.Value.Height + 2);
            using MemoryStream packetOut = new();
            packetOut.WriteS32V(ProtocolPackets.OutgoingPlayChunkLight);
            packetOut.WriteS32(x);
            packetOut.WriteS32(z);
            SemiCompactArray maskSkyLight = new(1, skyLight.Length);
            SemiCompactArray maskSkyLightEmpty = new(1, skyLight.Length);
            SemiCompactArray maskBlockLight = new(1, blockLight.Length);
            SemiCompactArray maskBlockLightEmpty = new(1, blockLight.Length);
            int numSkyLight = 0;
            for (int i = 0; i < skyLight.Length; i++)
            {
                SemiCompactArray? light = skyLight[i];
                if (light is not null)
                {
                    Contract.Assert(light.Bits == 4);
                    Contract.Assert(light.Length == 4096);
                    numSkyLight++;
                    maskSkyLight[i] = light.Data is not null ? 1 : 0;
                    maskSkyLightEmpty[i] = light.Data is null ? 1 : 0;
                }
            }
            int numBlockLight = 0;
            for (int i = 0; i < blockLight.Length; i++)
            {
                SemiCompactArray? light = blockLight[i];
                if (light is not null)
                {
                    Contract.Assert(light.Bits == 4);
                    Contract.Assert(light.Length == 4096);
                    numBlockLight++;
                    maskBlockLight[i] = light.Data is not null ? 1 : 0;
                    maskBlockLightEmpty[i] = light.Data is null ? 1 : 0;
                }
            }
            packetOut.WriteU64A(maskSkyLight.Data, SizePrefix.S32V);
            packetOut.WriteU64A(maskBlockLight.Data, SizePrefix.S32V);
            packetOut.WriteU64A(maskSkyLightEmpty.Data, SizePrefix.S32V);
            packetOut.WriteU64A(maskBlockLightEmpty.Data, SizePrefix.S32V);
            packetOut.WriteS32V(numSkyLight);
            for (int i = 0; i < skyLight.Length; i++)
            {
                if (maskSkyLight[i] == 0) continue;
                packetOut.WriteU8A(MemoryMarshal.AsBytes<ulong>(skyLight[i]!.Data).ToArray(), SizePrefix.S32V);
            }
            packetOut.WriteS32V(numBlockLight);
            for (int i = 0; i < blockLight.Length; i++)
            {
                if (maskBlockLight[i] == 0) continue;
                packetOut.WriteU8A(MemoryMarshal.AsBytes<ulong>(blockLight[i]!.Data).ToArray(), SizePrefix.S32V);
            }
            return SendAsync(packetOut.ToArray());
        }
        public Task SendChunkFullAsync(int x, int z, int[][] blocks, int[][] biomes, SemiCompactArray?[] skyLight, SemiCompactArray?[] blockLight, SemiCompactArray motionBlocking)
        {
            Contract.Assert(State == ProtocolState.Play);
            Contract.Assert(DimensionType.HasValue);
            Contract.Assert(DimensionName is not null);
            Contract.Assert(Biomes is not null);
            Contract.Assert(x < 2097152);
            Contract.Assert(z < 2097152);
            Contract.Assert(x >= -2097152);
            Contract.Assert(z >= -2097152);
            Contract.Assert(blocks.Length * 16 == DimensionType!.Value.Height);
            Contract.Assert(biomes.Length * 16 == DimensionType!.Value.Height);
            Contract.Assert(blockLight.Length * 16 == DimensionType!.Value.Height + 32);
            Contract.Assert(skyLight.Length * 16 == DimensionType!.Value.Height + 32);
            Contract.Assert(motionBlocking.Bits == (int)Math.Ceiling(Math.Log2(DimensionType!.Value.Height)));
            Contract.Assert(motionBlocking.Length == 256);
            using MemoryStream packetOut = new();
            packetOut.WriteS32V(ProtocolPackets.OutgoingPlayChunkFull);
            packetOut.WriteS32(x);
            packetOut.WriteS32(z);
            packetOut.WriteU8(0x0A);
            packetOut.WriteU8(12);
            packetOut.WriteString("MOTION_BLOCKING", SizePrefix.S16);
            packetOut.WriteS32(motionBlocking.Data.Length);
            packetOut.WriteU64A(motionBlocking.Data);
            packetOut.WriteU8(0);
            using MemoryStream dataOut = new();
            for (int i = 0; i < blocks.Length; i++)
            {
                int[] block = blocks[i];
                Contract.Assert(block.Length == 4096);
                Dictionary<int, int> blockCounts = [];
                ushort count = 0;
                for (int e = 0; e < 4096; e++)
                {
                    int id = Math.Clamp(block[e], 0, 32768); //TODO: proper auto updating value
                    if (id != 0) count++; //TODO: support other air block types
                    blockCounts.TryAdd(id, 0);
                    blockCounts[id]++;
                }
                dataOut.WriteU16(count);
                byte blockBits = (byte)Math.Ceiling(Math.Log2(blockCounts.Count));
                byte blockBitsTotal = (byte)Math.Ceiling(Math.Log2(32768)); //TODO: proper auto updating value
                if (blockBits > 8)
                {
                    SemiCompactArray data = new(blockBitsTotal, 4096);
                    dataOut.WriteU8(data.Bits);
                    for (int e = 0; e < data.Length; e++) data[e] = Math.Clamp(block[e], 0, 32768); //TODO: proper auto updating value
                    dataOut.WriteU64A(data.Data, SizePrefix.S32V);
                }
                else
                if (blockBits > 0)
                {
                    byte usedBits = blockBits < 4 ? (byte)4 : blockBits;
                    dataOut.WriteU8(usedBits);
                    SemiCompactArray data = new(usedBits, 4096);
                    Dictionary<int, int> mappings = [];
                    dataOut.WriteS32V(blockCounts.Count);
                    foreach (int id in blockCounts.Keys)
                    {
                        mappings[id] = mappings.Count;
                        dataOut.WriteS32V(id);
                    } //TODO: below: fix evaluation time of clamp thing and in other occurences too
                    for (int e = 0; e < 4096; e++) data[e] = mappings[Math.Clamp(block[e], 0, 32768)]; //TODO: proper auto updating value
                    dataOut.WriteU64A(data.Data, SizePrefix.S32V);
                }
                else
                {
                    dataOut.WriteU8(0);
                    dataOut.WriteS32V(blockCounts.First().Key);
                    dataOut.WriteS32V(0);
                }
                int[] biome = biomes[i];
                Contract.Assert(biome.Length == 64);
                Dictionary<int, int> biomeCounts = [];
                for (int e = 0; e < 64; e++)
                {
                    int id = Math.Clamp(biome[e], 0, Biomes!.Length);
                    biomeCounts.TryAdd(id, 0);
                    biomeCounts[id]++;
                }
                byte biomeBits = (byte)Math.Ceiling(Math.Log2(biomeCounts.Count));
                byte biomeBitsTotal = (byte)Math.Ceiling(Math.Log2(Biomes!.Length));
                if (biomeBits >= 4)
                {
                    dataOut.WriteU8(biomeBitsTotal);
                    SemiCompactArray data = new(biomeBitsTotal, 64);
                    for (int e = 0; e < data.Length; e++) data[i] = Math.Clamp(biome[e], 0, Biomes.Length);
                    dataOut.WriteU64A(data.Data, SizePrefix.S32V);
                }
                else
                if (biomeBits > 0)
                {
                    dataOut.WriteU8(biomeBits);
                    SemiCompactArray data = new(biomeBits, 64);
                    Dictionary<int, int> mappings = [];
                    dataOut.WriteS32V(biomeCounts.Count);
                    foreach (int id in biomeCounts.Keys)
                    {
                        mappings[id] = mappings.Count;
                        dataOut.WriteS32V(id);
                    }
                    for (int e = 0; e < data.Length; e++) data[i] = mappings[Math.Clamp(biome[e], 0, Biomes.Length)];
                    dataOut.WriteU64A(data.Data, SizePrefix.S32V);
                }
                else
                {
                    dataOut.WriteU8(0);
                    dataOut.WriteS32V(biomeCounts.First().Key);
                    dataOut.WriteS32V(0);
                }
            }
            packetOut.WriteU8A(dataOut.ToArray(), SizePrefix.S32V);
            packetOut.WriteS32V(0); //TODO: block entities
            SemiCompactArray maskSkyLight = new(1, skyLight.Length);
            SemiCompactArray maskSkyLightEmpty = new(1, skyLight.Length);
            SemiCompactArray maskBlockLight = new(1, blockLight.Length);
            SemiCompactArray maskBlockLightEmpty = new(1, blockLight.Length);
            int numSkyLight = 0;
            for (int i = 0; i < skyLight.Length; i++)
            {
                SemiCompactArray? light = skyLight[i];
                if (light is not null)
                {
                    Contract.Assert(light.Bits == 4);
                    Contract.Assert(light.Length == 4096);
                    numSkyLight++;
                    maskSkyLight[i] = light.Data is not null ? 1 : 0;
                    maskSkyLightEmpty[i] = light.Data is null ? 1 : 0;
                }
            }
            int numBlockLight = 0;
            for (int i = 0; i < blockLight.Length; i++)
            {
                SemiCompactArray? light = blockLight[i];
                if (light is not null)
                {
                    Contract.Assert(light.Bits == 4);
                    Contract.Assert(light.Length == 4096);
                    numBlockLight++;
                    maskBlockLight[i] = light.Data is not null ? 1 : 0;
                    maskBlockLightEmpty[i] = light.Data is null ? 1 : 0;
                }
            }
            packetOut.WriteU64A(maskSkyLight.Data, SizePrefix.S32V);
            packetOut.WriteU64A(maskBlockLight.Data, SizePrefix.S32V);
            packetOut.WriteU64A(maskSkyLightEmpty.Data, SizePrefix.S32V);
            packetOut.WriteU64A(maskBlockLightEmpty.Data, SizePrefix.S32V);
            packetOut.WriteS32V(numSkyLight);
            for (int i = 0; i < skyLight.Length; i++)
            {
                if (maskSkyLight[i] == 0) continue;
                packetOut.WriteU8A(MemoryMarshal.AsBytes<ulong>(skyLight[i]!.Data).ToArray(), SizePrefix.S32V);
            }
            packetOut.WriteS32V(numBlockLight);
            for (int i = 0; i < blockLight.Length; i++)
            {
                if (maskBlockLight[i] == 0) continue;
                packetOut.WriteU8A(MemoryMarshal.AsBytes<ulong>(blockLight[i]!.Data).ToArray(), SizePrefix.S32V);
            }
            return SendAsync(packetOut.ToArray());
        }
        public Task SendChunkUnloadAsync(int x, int z)
        {
            Contract.Assert(State == ProtocolState.Play);
            Contract.Assert(DimensionName is not null);
            Contract.Assert(x < 2097152);
            Contract.Assert(z < 2097152);
            Contract.Assert(x >= -2097152);
            Contract.Assert(z >= -2097152);
            using MemoryStream packetOut = new();
            packetOut.WriteS32V(ProtocolPackets.OutgoingPlayChunkUnload);
            packetOut.WriteS32(x);
            packetOut.WriteS32(z);
            return SendAsync(packetOut.ToArray());
        }
        public Task SendChunkCenterAsync(int x, int z)
        {
            Contract.Assert(State == ProtocolState.Play);
            Contract.Assert(DimensionName is not null);
            Contract.Assert(x < 2097152);
            Contract.Assert(z < 2097152);
            Contract.Assert(x >= -2097152);
            Contract.Assert(z >= -2097152);
            using MemoryStream packetOut = new();
            packetOut.WriteS32V(ProtocolPackets.OutgoingPlayChunkCenter);
            packetOut.WriteS32V(x);
            packetOut.WriteS32V(z);
            return SendAsync(packetOut.ToArray());
        }
        public Task SendChunkWaitAsync()
        {
            Contract.Assert(State == ProtocolState.Play);
            Contract.Assert(DimensionName is not null);
            using MemoryStream packetOut = new();
            packetOut.WriteS32V(ProtocolPackets.OutgoingPlayEvent);
            packetOut.WriteU8(13);
            packetOut.WriteF32(0.0f);
            return SendAsync(packetOut.ToArray());
        }
        public Task SendBlockSingleAsync(Position position, int id)
        {
            Contract.Assert(State == ProtocolState.Play);
            Contract.Assert(DimensionName is not null);
            using MemoryStream packetOut = new();
            packetOut.WriteS32V(ProtocolPackets.OutgoingPlayBlockSingle);
            packetOut.WriteU64(position.Data);
            packetOut.WriteS32V(id);
            return SendAsync(packetOut.ToArray());
        }
        public Task SendEntityAddAsync(int eid, Guid id, EntityBase entity, double x, double y, double z, float pitch, float yaw, float headYaw)
        {
            Contract.Assert(State == ProtocolState.Play);
            Contract.Assert(DimensionName is not null);
            using MemoryStream packetOut = new();
            packetOut.WriteS32V(ProtocolPackets.OutgoingPlayEntityAdd);
            packetOut.WriteS32V(eid);
            packetOut.WriteGuid(id);
            packetOut.WriteS32V(entity.Id);
            packetOut.WriteF64(x);
            packetOut.WriteF64(y);
            packetOut.WriteF64(z);
            packetOut.WriteU8((byte)(pitch / 360.0f * 256));
            packetOut.WriteU8((byte)(yaw / 360.0f * 256));
            packetOut.WriteU8((byte)(headYaw / 360.0f * 256));
            packetOut.WriteS32V(entity.InitialData);
            packetOut.WriteS16(0);
            packetOut.WriteS16(0);
            packetOut.WriteS16(0);
            return SendAsync(packetOut.ToArray());
        }
        public Task SendEntityRemoveAsync(int[] eid)
        {
            Contract.Assert(State == ProtocolState.Play);
            Contract.Assert(DimensionName is not null);
            using MemoryStream packetOut = new();
            packetOut.WriteS32V(ProtocolPackets.OutgoingPlayEntityRemove);
            packetOut.WriteS32V(eid.Length);
            for (int i = 0; i < eid.Length; i ++) packetOut.WriteS32V(eid[i]);
            return SendAsync(packetOut.ToArray());
        }
        public Task SendEntityAnimationAsync(int eid, EntityAnimation animation)
        {
            Contract.Assert(State == ProtocolState.Play);
            Contract.Assert(DimensionName is not null);
            using MemoryStream packetOut = new();
            packetOut.WriteS32V(ProtocolPackets.OutgoingPlayEntityAnimation);
            packetOut.WriteS32V(eid);
            packetOut.WriteS32V((int)animation);
            return SendAsync(packetOut.ToArray());
        }
        public async Task SendEntityPositionAsync(int eid, (double x, double y, double z, float yaw, float pitch, float headYaw) current, (double x, double y, double z, float yaw, float pitch, float headYaw)? previous = null)
        {
            Contract.Assert(State == ProtocolState.Play);
            Contract.Assert(DimensionName is not null);
            double dx = previous.HasValue ? current.x - previous.Value.x : double.PositiveInfinity;
            double dy = previous.HasValue ? current.y - previous.Value.y : double.PositiveInfinity;
            double dz = previous.HasValue ? current.z - previous.Value.z : double.PositiveInfinity;
            bool dr = previous.HasValue ? (current.yaw != previous.Value.yaw || current.pitch != previous.Value.pitch) : true;
            if (Math.Abs(dx) >= 8 || Math.Abs(dy) >= 8 || Math.Abs(dz) >= 8)
            {
                using MemoryStream packetOut = new();
                packetOut.WriteS32V(ProtocolPackets.OutgoingPlayEntityPositionFar);
                packetOut.WriteS32V(eid);
                packetOut.WriteF64(current.x);
                packetOut.WriteF64(current.y);
                packetOut.WriteF64(current.z);
                packetOut.WriteU8((byte)(current.yaw / 360.0f * 256));
                packetOut.WriteU8((byte)(current.pitch / 360.0f * 256));
                packetOut.WriteBool(false);
                await SendAsync(packetOut.ToArray());
            }
            else if (dr && (dx != 0 || dy != 0 || dz != 0))
            {
                using MemoryStream packetOut = new();
                packetOut.WriteS32V(ProtocolPackets.OutgoingPlayEntityPositionShort);
                packetOut.WriteS32V(eid);
                packetOut.WriteS16((short)(dx * 4096));
                packetOut.WriteS16((short)(dy * 4096));
                packetOut.WriteS16((short)(dz * 4096));
                packetOut.WriteU8((byte)(current.yaw / 360.0f * 256));
                packetOut.WriteU8((byte)(current.pitch / 360.0f * 256));
                packetOut.WriteBool(false);
                await SendAsync(packetOut.ToArray());
            }
            else if (dx != 0 || dy != 0 || dz != 0)
            {
                using MemoryStream packetOut = new();
                packetOut.WriteS32V(ProtocolPackets.OutgoingPlayEntityLocationShort);
                packetOut.WriteS32V(eid);
                packetOut.WriteS16((short)(dx * 4096));
                packetOut.WriteS16((short)(dy * 4096));
                packetOut.WriteS16((short)(dz * 4096));
                packetOut.WriteBool(false);
                await SendAsync(packetOut.ToArray());
            }
            else if (dr)
            {
                using MemoryStream packetOut = new();
                packetOut.WriteS32V(ProtocolPackets.OutgoingPlayEntityRotation);
                packetOut.WriteS32V(eid);
                packetOut.WriteU8((byte)(current.yaw / 360.0f * 256));
                packetOut.WriteU8((byte)(current.pitch / 360.0f * 256));
                packetOut.WriteBool(false);
                await SendAsync(packetOut.ToArray());
            }
            if (previous.HasValue ? current.headYaw != previous.Value.headYaw : true)
            {
                using MemoryStream packetOut = new();
                packetOut.WriteS32V(ProtocolPackets.OutgoingPlayEntityHead);
                packetOut.WriteS32V(eid);
                packetOut.WriteU8((byte)(current.headYaw / 360.0f * 256));
                await SendAsync(packetOut.ToArray());
            }
        }
        public Task SendEntityDataAsync(int eid, EntityBase entity, EntityBase? previous = null)
        {
            Contract.Assert(State == ProtocolState.Play);
            Contract.Assert(DimensionName is not null);
            using MemoryStream packetOut = new();
            packetOut.WriteS32V(ProtocolPackets.OutgoingPlayEntityData);
            packetOut.WriteS32V(eid);
            entity.Serialize(packetOut, previous);
            packetOut.WriteU8(0xFF);
            return SendAsync(packetOut.ToArray());
        }
        public Task SendEffectAddAsync(int eid, Effect effect, int level, int duration, bool beacon, bool particles, bool icon, bool blend)
        {
            Contract.Assert(State == ProtocolState.Play);
            Contract.Assert(DimensionName is not null);
            using MemoryStream packetOut = new();
            packetOut.WriteS32V(ProtocolPackets.OutgoingPlayEffectAdd);
            packetOut.WriteS32V(eid);
            packetOut.WriteS32V((int)effect);
            packetOut.WriteS32V(level);
            packetOut.WriteS32V(duration);
            byte flags = 0x00;
            if (beacon) flags |= 0x01;
            if (particles) flags |= 0x02;
            if (icon) flags |= 0x04;
            if (blend) flags |= 0x08;
            packetOut.WriteU8(flags);
            return SendAsync(packetOut.ToArray());
        }
        public Task SendEffectRemoveAsync(int eid, Effect effect)
        {
            Contract.Assert(State == ProtocolState.Play);
            Contract.Assert(DimensionName is not null);
            using MemoryStream packetOut = new();
            packetOut.WriteS32V(ProtocolPackets.OutgoingPlayEffectRemove);
            packetOut.WriteS32V(eid);
            packetOut.WriteS32V((int)effect);
            return SendAsync(packetOut.ToArray());
        }
        public Task SendHotbarAsync(int slot)
        {
            Contract.Assert(State == ProtocolState.Play);
            Contract.Assert(DimensionName is not null);
            Contract.Assert(slot >= 0 && slot < 9);
            using MemoryStream packetOut = new();
            packetOut.WriteS32V(ProtocolPackets.OutgoingPlayHotbar);
            packetOut.WriteS8((sbyte)slot);
            return SendAsync(packetOut.ToArray());
        }
        public Task SendContainerSingleAsync(sbyte id, short slot, Item item)
        {
            Contract.Assert(State == ProtocolState.Play);
            Contract.Assert(DimensionName is not null);
            using MemoryStream packetOut = new();
            packetOut.WriteS32V(ProtocolPackets.OutgoingPlayContainerSingle);
            packetOut.WriteS8(id);
            packetOut.WriteS32V(++ContainerSequence);
            packetOut.WriteS16(slot);
            item.Serialize(packetOut);
            return SendAsync(packetOut.ToArray());
        }
        public Task SendContainerFullAsync(sbyte id, Item[] content, Item carry)
        {
            Contract.Assert(State == ProtocolState.Play);
            Contract.Assert(DimensionName is not null);
            using MemoryStream packetOut = new();
            packetOut.WriteS32V(ProtocolPackets.OutgoingPlayContainerFull);
            packetOut.WriteS8(id);
            packetOut.WriteS32V(++ContainerSequence);
            packetOut.WriteS32V(content.Length);
            for (int i = 0; i < content.Length; i++)
            {
                content[i].Serialize(packetOut);
            }
            carry.Serialize(packetOut);
            return SendAsync(packetOut.ToArray());
        }
        public Task SendSpawnpointAsync(Position position, float angle)
        {
            Contract.Assert(State == ProtocolState.Play);
            Contract.Assert(DimensionName is not null);
            using MemoryStream packetOut = new();
            packetOut.WriteS32V(ProtocolPackets.OutgoingPlaySpawnpoint);
            packetOut.WriteU64(position.Data);
            packetOut.WriteF32(angle);
            return SendAsync(packetOut.ToArray());
        }
        public Task SendPlayerPositionAsync(double x, double y, double z, float yaw, float pitch, int id)
        {
            Contract.Assert(State == ProtocolState.Play);
            Contract.Assert(DimensionName is not null);
            using MemoryStream packetOut = new();
            packetOut.WriteS32V(ProtocolPackets.OutgoingPlayPlayerPosition);
            packetOut.WriteF64(x);
            packetOut.WriteF64(y);
            packetOut.WriteF64(z);
            packetOut.WriteF32(yaw);
            packetOut.WriteF32(pitch);
            packetOut.WriteU8(0);
            packetOut.WriteS32V(id);
            return SendAsync(packetOut.ToArray());
        }
        public Task SendPlayerPointBlockAsync(double x, double y, double z, bool eyes)
        {
            Contract.Assert(State == ProtocolState.Play);
            Contract.Assert(DimensionName is not null);
            using MemoryStream packetOut = new();
            packetOut.WriteS32V(ProtocolPackets.OutgoingPlayPlayerPoint);
            packetOut.WriteS32V(eyes ? 1 : 0);
            packetOut.WriteF64(x);
            packetOut.WriteF64(y);
            packetOut.WriteF64(z);
            packetOut.WriteBool(false);
            return SendAsync(packetOut.ToArray());
        }
        public Task SendPlayerPointEntityAsync(double x, double y, double z, bool eyes, int eid, bool entityEyes)
        {
            Contract.Assert(State == ProtocolState.Play);
            Contract.Assert(DimensionName is not null);
            using MemoryStream packetOut = new();
            packetOut.WriteS32V(ProtocolPackets.OutgoingPlayPlayerPoint);
            packetOut.WriteS32V(eyes ? 1 : 0);
            packetOut.WriteF64(x);
            packetOut.WriteF64(y);
            packetOut.WriteF64(z);
            packetOut.WriteBool(true);
            packetOut.WriteS32V(eid);
            packetOut.WriteS32V(entityEyes ? 1 : 0);
            return SendAsync(packetOut.ToArray());
        }
        private async Task ListenAsync()
        {
            try
            {
                disconnected = false;
                while (true) //TODO: disconnect
                {
                    PacketQueue.Enqueue(await ReceiveAsync());
                }
            }
            finally
            {
                disconnected = true;
                Stream.Close();
            }
        }
        private async Task<byte[]> ReceiveAsync()
        {
            byte[] data = await Stream.ReadU8AAsync(SizePrefix.S32V);
            if (CompressionThreshold < 0) return data;
            using MemoryStream packetIn = new(data);
            int size = packetIn.ReadS32V();
            if (size <= 0) return packetIn.ReadU8A(data.Length - (int)packetIn.Position);
            using ZLibStream zlib = new(packetIn, CompressionMode.Decompress, false);
            return zlib.ReadU8A(size);
        }
        private async Task SendAsync(byte[] data)
        {
            await Sync.WaitAsync();
            try
            {
                if (CompressionThreshold < 0) await Stream.WriteU8AAsync(data, SizePrefix.S32V);
                else
                {
                    if (data.Length < CompressionThreshold)
                    {
                        await Stream.WriteS32VAsync(data.Length + 1);
                        await Stream.WriteU8Async(0);
                        await Stream.WriteU8AAsync(data);
                    }
                    else
                    {
                        using MemoryStream packetStream = new();
                        using (ZLibStream zlib = new(packetStream, CompressionLevel, true)) zlib.Write(data);
                        byte[] compressed = packetStream.ToArray();
                        int extra = 0;
                        for (int value = data.Length; value != 0; value >>= 7) extra++;
                        await Stream.WriteS32VAsync(compressed.Length + extra);
                        await Stream.WriteS32VAsync(data.Length);
                        await Stream.WriteU8AAsync(compressed);
                    }
                }
            }
            finally
            {
                Sync.Release();
            }
        }

        [GeneratedRegex("^[a-zA-Z0-9_]{1,16}$")]
        private static partial Regex MinecraftUsernameRegex();

        [GeneratedRegex("^[a-zA-Z0-9_]+:[a-zA-Z0-9_]+$")]
        internal static partial Regex NamespaceRegex();
    }
}
