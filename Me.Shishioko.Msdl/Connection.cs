using Me.Shishioko.Msdl.Data;
using Net.Myzuc.ShioLib;
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
using System.Text.RegularExpressions;
using System.Diagnostics.Contracts;
using Me.Shishioko.Msdl.Data.Chat;
using System.Collections.Concurrent;
using System.Runtime.InteropServices;

namespace Me.Shishioko.Msdl
{
    public sealed partial class Connection
    {
        private Stream Stream;
        private ProtocolState State = ProtocolState.Handshake;
        private int CompressionThreshold = -1;
        private CompressionLevel CompressionLevel = CompressionLevel.Optimal;

        private ConcurrentQueue<byte[]> PacketQueue = new();
        private Dimension[]? DimensionTypes = null;
        private Dimension? DimensionType = null;
        private string[]? DimensionNames = null;
        private string? DimensionName = null;
        private IReadOnlyList<Biome>? Biomes = null;
        //TODO: damage types?
        private bool Hardcore = false;
        private bool RespawnScreen = false;

        public event Func<Task> ReceiveConfigurationEnd = () => Task.CompletedTask;
        public event Func<string, byte[], Task> ReceiveConfigurationMessageAsync = (string channel, byte[] data) => Task.CompletedTask;
        public Connection(Stream stream)
        {
            Stream = stream;
        }
        public async Task<(ProtocolState, string, ushort)> ReceiveHandshakeAsync()
        {
            Contract.Requires(State == ProtocolState.Handshake);
            using MemoryStream packetIn = new(await ReceiveAsync());
            if (packetIn.ReadS32V() != 0x00) throw new ProtocolViolationException("Expected handshake packet");
            int version = packetIn.ReadS32V();
            string address = packetIn.ReadString(SizePrefix.S32V);
            ushort port = packetIn.ReadU16();
            int state = packetIn.ReadS32V();
            if (state == 1) State = ProtocolState.Status;
            else if (state == 2) State = ProtocolState.Login;
            else throw new ProtocolViolationException();
            return (State, address, port);
        }
        public async Task<bool> ExchangeStatusStatusAsync(ServerStatus status)
        {
            Contract.Requires(State == ProtocolState.Status);
            using MemoryStream packetIn = new(await ReceiveAsync());
            if (packetIn.ReadS32V() != 0x00) return false;
            using MemoryStream packetOut = new();
            packetOut.WriteS32V(0x00);
            packetOut.WriteString(JsonConvert.SerializeObject(status, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore }), SizePrefix.S32V);
            await SendAsync(packetOut.ToArray());
            return true;
        }
        public async Task<bool> ExchangeStatusPingAsync()
        {
            Contract.Requires(State == ProtocolState.Status);
            using MemoryStream packetIn = new(await ReceiveAsync());
            if (packetIn.ReadS32V() != 0x01) return false;
            using MemoryStream packetOut = new();
            packetOut.WriteS32V(0x01);
            packetOut.WriteU64(packetIn.ReadU64());
            await SendAsync(packetOut.ToArray());
            return true;
        }
        public async Task<(string, Guid)> ReceiveLoginStartAsync()
        {
            Contract.Requires(State == ProtocolState.Login);
            using MemoryStream packetIn = new(await ReceiveAsync());
            if (packetIn.ReadS32V() != 0x00) throw new ProtocolViolationException();
            return (packetIn.ReadString(SizePrefix.S32V), packetIn.ReadGuid());
        }
        public async Task SendLoginDisconnectAsync(ChatComponent message)
        {
            Contract.Requires(State == ProtocolState.Login);
            using MemoryStream packetOut = new();
            packetOut.WriteS32V(0x00);
            packetOut.WriteString(JsonConvert.SerializeObject(message, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore }), SizePrefix.S32V);
            await SendAsync(packetOut.ToArray());
        }
        public async Task<Property[]?> ExchangeLoginEncryptionAsync(Guid guid, string name, string server)
        {
            Contract.Requires(State == ProtocolState.Login);
            Contract.Requires(name.Length <= 16);
            Contract.Requires(server.Length <= 20);
            byte[] verify = RandomNumberGenerator.GetBytes(4);
            using RSA rsa = RSA.Create();
            using (MemoryStream packetOut = new())
            {
                packetOut.WriteS32V(0x01);
                packetOut.WriteString(server, SizePrefix.S32V);
                packetOut.WriteU8A(rsa.ExportSubjectPublicKeyInfo(), SizePrefix.S32V);
                packetOut.WriteU8A(verify, SizePrefix.S32V);
                await SendAsync(packetOut.ToArray());
            }
            using MemoryStream packetIn = new(await ReceiveAsync());
            if (packetIn.ReadS32V() != 0x01) throw new ProtocolViolationException();
            byte[] secret = rsa.Decrypt(packetIn.ReadU8A(SizePrefix.S32V), RSAEncryptionPadding.Pkcs1);
            if (!rsa.Decrypt(packetIn.ReadU8A(SizePrefix.S32V), RSAEncryptionPadding.Pkcs1).SequenceEqual(verify)) throw new ProtocolViolationException();
            Stream = new AesCfbStream(Stream, secret, secret, false);
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
            List<Property> properties = [];
            return auth.RootElement.GetProperty("properties")!.EnumerateArray().Select(property => new Property(property.GetProperty("name").GetString()!, property.GetProperty("value").GetString()!, property.GetProperty("signature").GetString())).ToArray();
        }
        public async Task SendLoginCompressionAsync(int compressionThreshold, CompressionLevel compressionLevel)
        {
            Contract.Requires(State == ProtocolState.Login);
            using MemoryStream packetOut = new();
            packetOut.WriteS32V(0x03);
            packetOut.WriteS32V(compressionThreshold);
            await SendAsync(packetOut.ToArray());
            CompressionThreshold = compressionThreshold;
            CompressionLevel = compressionLevel;
        }
        public async Task<byte[]> ExchangeLoginMessageAsync(string channel, byte[] data)
        {
            Contract.Requires(State == ProtocolState.Login);
            using MemoryStream packetOut = new();
            packetOut.WriteS32V(0x04);
            packetOut.WriteS32V(0);
            packetOut.WriteString(channel, SizePrefix.S32V);
            packetOut.WriteU8A(data);
            await SendAsync(packetOut.ToArray());
            using MemoryStream packetIn =new(await ReceiveAsync());
            if (packetIn.ReadS32V() != 0x02) throw new ProtocolViolationException();
            if (packetIn.ReadS32V() != 0) throw new ProtocolViolationException();
            if (packetIn.ReadString(SizePrefix.S32V) != channel) throw new ProtocolViolationException();
            return packetIn.ReadU8A((int)(packetIn.Length - packetIn.Position));
        }
        public async Task ExchangeLoginEndAsync(Guid guid, string name, Property[] properties)
        {
            Contract.Requires(State == ProtocolState.Login);
            Contract.Requires(name.Length <= 16);
            using MemoryStream packetOut = new();
            packetOut.WriteS32V(0x02);
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
            await SendAsync(packetOut.ToArray());
            using MemoryStream packetIn =new(await ReceiveAsync());
            if (packetIn.ReadS32V() != 0x03) throw new ProtocolViolationException();
            State = ProtocolState.Configuration;
            _ = ListenAsync();
        }
        public async Task ProcessAsync()
        {
            List<byte[]> packets = [];
            while (true)
            {
                if (!PacketQueue.TryDequeue(out byte[]? packet)) break; //TODO: async
                packets.Add(packet);
            }
            foreach (byte[] packetDataIn in packets)
            {
                using MemoryStream packetIn = new(packetDataIn);
                if (State == ProtocolState.Configuration)
                {
                    switch (packetIn.ReadS32V())
                    {
                        case 0x00:
                            {
                                break; //TODO:
                            }
                        case 0x01:
                            {
                                string channel = packetIn.ReadString(SizePrefix.S32V);
                                if (!Connection.NamespaceRegex().IsMatch(channel)) throw new ProtocolViolationException();
                                byte[] data = packetIn.ReadU8A((int)(packetIn.Length - packetIn.Position));
                                await ReceiveConfigurationMessageAsync(channel, data);
                                break;
                            }
                        case 0x02:
                            {
                                await ReceiveConfigurationEnd();
                                State = ProtocolState.Play;
                                break;
                            }
                        case 0x03:
                            {
                                break;
                            }
                        default:
                            {
                                throw new ProtocolViolationException();
                            }
                    }
                }
                else if (State == ProtocolState.Play)
                {
                    switch (packetIn.ReadS32V())
                    {
                        default:
                            {
                                //throw new ProtocolViolationException(); //TODO: remove comment
                                break;
                            }
                    }
                }
            }
        }
        public async Task SendConfigurationRegistriesAsync(Dimension[] dimensionTypes, Biome[] biomes) //TODO: other registries
        {
            Contract.Requires(State == ProtocolState.Configuration);
            Contract.Requires(dimensionTypes.Length > 0);
            Contract.Requires(biomes.Length > 0);
            DimensionTypes = dimensionTypes;
            Biomes = biomes;
            using MemoryStream packetOut = new();
            packetOut.WriteS32V(0x05);
            packetOut.WriteU8(10);
            
            packetOut.WriteU8(10);
            packetOut.WriteString("minecraft:damage_type", SizePrefix.S16);
            packetOut.WriteU8(8);
            packetOut.WriteString("type", SizePrefix.S16);
            packetOut.WriteString("minecraft:damage_type", SizePrefix.S16);
            packetOut.WriteU8(9);
            packetOut.WriteString("value", SizePrefix.S16);
            packetOut.WriteU8(10);
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
            packetOut.WriteS32(damages.Length);
            for (int i = 0; i < damages.Length; i++)
            {
                packetOut.WriteU8(8);
                packetOut.WriteString("name", SizePrefix.S16);
                packetOut.WriteString(damages[i], SizePrefix.S16);
                packetOut.WriteU8(3);
                packetOut.WriteString("id", SizePrefix.S16);
                packetOut.WriteS32(i);
                packetOut.WriteU8(10);
                packetOut.WriteString("element", SizePrefix.S16);
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
                packetOut.WriteU8(0);
            }
            packetOut.WriteU8(0);

            packetOut.WriteU8(10);
            packetOut.WriteString("minecraft:dimension_type", SizePrefix.S16);
            packetOut.WriteU8(8);
            packetOut.WriteString("type", SizePrefix.S16);
            packetOut.WriteString("minecraft:dimension_type", SizePrefix.S16);
            packetOut.WriteU8(9);
            packetOut.WriteString("value", SizePrefix.S16);
            packetOut.WriteU8(10);
            packetOut.WriteS32(DimensionTypes.Length);
            for (int i = 0; i < dimensionTypes.Length; i++)
            {
                Dimension dimensionType = DimensionTypes[i];
                packetOut.WriteU8(8);
                packetOut.WriteString("name", SizePrefix.S16);
                packetOut.WriteString(dimensionType.Name, SizePrefix.S16);
                packetOut.WriteU8(3);
                packetOut.WriteString("id", SizePrefix.S16);
                packetOut.WriteS32(i);
                packetOut.WriteU8(10);
                packetOut.WriteString("element", SizePrefix.S16);
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
                packetOut.WriteU8(0);
            }
            packetOut.WriteU8(0);
            packetOut.WriteU8(10);
            packetOut.WriteString("minecraft:worldgen/biome", SizePrefix.S16);
            packetOut.WriteU8(8);
            packetOut.WriteString("type", SizePrefix.S16);
            packetOut.WriteString("minecraft:worldgen/biome", SizePrefix.S16);
            packetOut.WriteU8(9);
            packetOut.WriteString("value", SizePrefix.S16);
            packetOut.WriteU8(10);
            packetOut.WriteS32(Biomes.Count);
            uint bid = 0;
            foreach (Biome biome in Biomes)
            {
                packetOut.WriteU8(8);
                packetOut.WriteString("name", SizePrefix.S16);
                packetOut.WriteString(biome.Name, SizePrefix.S16);
                packetOut.WriteU8(3);
                packetOut.WriteString("id", SizePrefix.S16);
                packetOut.WriteU32(bid++);
                packetOut.WriteU8(10);
                packetOut.WriteString("element", SizePrefix.S16);
                packetOut.WriteU8(1);
                packetOut.WriteString("has_precipitation", SizePrefix.S16);
                packetOut.WriteBool(biome.Precipitation);
                packetOut.WriteU8(5);
                packetOut.WriteString("temperature", SizePrefix.S16);
                packetOut.WriteF32(0.0f);
                packetOut.WriteU8(5);
                packetOut.WriteString("downfall", SizePrefix.S16);
                packetOut.WriteF32(0.0f);
                packetOut.WriteU8(10);
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
                    packetOut.WriteU8(10);
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
                    packetOut.WriteU8(10);
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
                    packetOut.WriteU8(10);
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
                packetOut.WriteU8(0);
            }
            packetOut.WriteU8(0);
            packetOut.WriteU8(0);
            await SendAsync(packetOut.ToArray());
        }
        public async Task SendConfigurationEndAsync()
        {
            Contract.Requires(State == ProtocolState.Configuration);
            using MemoryStream packetOut = new();
            packetOut.WriteS32V(0x02);
            await SendAsync(packetOut.ToArray());
        }
        public async Task SendPlayLoginAsync(int entityID, bool hardcore, int viewDistance, int simulationDistance, bool reducedDebug, bool respawnScreen, string[] dimensionNames, Dimension dimensionType, string dimensionName, ulong seedHash, Gamemode currentGamemode, Gamemode? previousGamemode, bool flatWorld, DeathLocation? deathLocation)
        {
            Contract.Requires(State == ProtocolState.Play);
            Contract.Requires(DimensionTypes is not null);
            Contract.Requires(dimensionNames is not null);
            Contract.Requires(viewDistance >= 0);
            Contract.Requires(simulationDistance >= 0);
            Contract.Requires(DimensionTypes!.Contains(dimensionType));
            Contract.Requires(dimensionNames!.Contains(dimensionName));
            DimensionNames = dimensionNames;
            using MemoryStream packetOut = new();
            packetOut.WriteS32V(0x29);
            packetOut.WriteS32(entityID);
            packetOut.WriteBool(Hardcore = hardcore);
            packetOut.WriteS32V(DimensionNames!.Length);
            for (int i = 0; i < DimensionNames.Length; i++) packetOut.WriteString(DimensionNames[i], SizePrefix.S32V);
            packetOut.WriteS32V(0);
            packetOut.WriteS32V(viewDistance);
            packetOut.WriteS32V(simulationDistance);
            packetOut.WriteBool(reducedDebug);
            packetOut.WriteBool(RespawnScreen = respawnScreen);
            packetOut.WriteBool(false);
            packetOut.WriteString((DimensionType = dimensionType).Value.Name, SizePrefix.S32V);
            packetOut.WriteString(DimensionName = dimensionName, SizePrefix.S32V);
            packetOut.WriteU64(seedHash);
            packetOut.WriteU8((byte)currentGamemode);
            packetOut.WriteS8(previousGamemode.HasValue ? (sbyte)previousGamemode.Value : (sbyte)-1);
            packetOut.WriteBool(false);
            packetOut.WriteBool(flatWorld);
            packetOut.WriteBool(deathLocation.HasValue);
            if (deathLocation.HasValue)
            {
                Contract.Requires(DimensionNames.Contains(deathLocation.Value.Dimension));
                packetOut.WriteString(deathLocation.Value.Dimension, SizePrefix.S32V);
                packetOut.WriteU64(deathLocation.Value.Location.Value);
            }
            packetOut.WriteS32V(0);
            await SendAsync(packetOut.ToArray());
        }
        public async Task SendPlayChunkBiomesAsync(int x, int z, int[][] biomes)
        {
            Contract.Requires(State == ProtocolState.Play);
            Contract.Requires(DimensionType.HasValue);
            Contract.Requires(DimensionName is not null);
            Contract.Requires(x < 2097152);
            Contract.Requires(z < 2097152);
            Contract.Requires(x >= -2097152);
            Contract.Requires(z >= -2097152);
            Contract.Requires(biomes.Length * 16 == DimensionType!.Value.Height);
            using MemoryStream packetOut = new();
            packetOut.WriteS32V(0x0E);
            packetOut.WriteS32V(1);
            packetOut.WriteS32(x);
            packetOut.WriteS32(z);
            using MemoryStream dataOut = new();
            for (int i = 0; i < biomes.Length; i++)
            {
                int[] biome = biomes[i];
                Contract.Requires(biome.Length == 64);
                Dictionary<int, int> counts = [];
                for (int e = 0; e < biome.Length; e++)
                {
                    int id = Math.Clamp(biome[e], 0, Biomes.Count);
                    counts.TryAdd(id, 0);
                    counts[id]++;
                }
                byte bits = (byte)Math.Log2(counts.Count);
                byte totalbits = (byte)Math.Log2(Biomes.Count);
                if (bits >= 4)
                {
                    dataOut.WriteU8(totalbits);
                    CompactArray data = new(totalbits, biome.Length);
                    for (int e = 0; e < data.Length; e++) data[i] = Math.Clamp(biome[e], 0, Biomes.Count);
                    dataOut.WriteU64A(data.Data, SizePrefix.S32V);
                }
                else
                if (bits > 0)
                {
                    dataOut.WriteU8(bits);
                    CompactArray data = new(bits < 4 ? (byte)4 : bits, biome.Length);
                    Dictionary<int, int> mappings = [];
                    dataOut.WriteS32V(counts.Count);
                    foreach (int id in counts.Keys)
                    {
                        mappings[id] = mappings.Count;
                        dataOut.WriteS32V(id);
                    }
                    for (int e = 0; e < data.Length; e++) data[i] = mappings[Math.Clamp(biome[e], 0, Biomes.Count)];
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
            await SendAsync(packetOut.ToArray());
        }
        public async Task SendPlayChunkLightAsync(int x, int z, ChunkSectionLight?[] skyLight, ChunkSectionLight?[] blockLight)
        {
            Contract.Requires(State == ProtocolState.Play);
            Contract.Requires(DimensionType.HasValue);
            Contract.Requires(DimensionName is not null);
            Contract.Requires(x < 2097152);
            Contract.Requires(z < 2097152);
            Contract.Requires(x >= -2097152);
            Contract.Requires(z >= -2097152);
            Contract.Requires(blockLight.Length * 16 == DimensionType!.Value.Height + 2);
            Contract.Requires(skyLight.Length * 16 == DimensionType!.Value.Height + 2);
            using MemoryStream packetOut = new();
            packetOut.WriteS32V(0x25);
            packetOut.WriteS32(x);
            packetOut.WriteS32(z);
            CompactArray maskSkyLight = new(1, skyLight.Length);
            CompactArray maskSkyLightEmpty = new(1, skyLight.Length);
            CompactArray maskBlockLight = new(1, blockLight.Length);
            CompactArray maskBlockLightEmpty = new(1, blockLight.Length);
            int numSkyLight = 0;
            for (int i = 0; i < skyLight.Length; i++)
            {
                ChunkSectionLight? light = skyLight[i];
                if (light.HasValue)
                {
                    numSkyLight++;
                    maskSkyLight[i] = light.Value.Data is not null ? 1 : 0;
                    maskSkyLightEmpty[i] = light.Value.Data is null ? 1 : 0;
                }
            }
            int numBlockLight = 0;
            for (int i = 0; i < blockLight.Length; i++)
            {
                ChunkSectionLight? light = blockLight[i];
                if (light.HasValue)
                {
                    numBlockLight++;
                    maskBlockLight[i] = light.Value.Data is not null ? 1 : 0;
                    maskBlockLightEmpty[i] = light.Value.Data is null ? 1 : 0;
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
                packetOut.WriteU8A(MemoryMarshal.AsBytes<ulong>(skyLight[i]!.Value.Data!.Data).ToArray(), SizePrefix.S32V);
            }
            packetOut.WriteS32V(numBlockLight);
            for (int i = 0; i < blockLight.Length; i++)
            {
                if (maskBlockLight[i] == 0) continue;
                packetOut.WriteU8A(MemoryMarshal.AsBytes<ulong>(blockLight[i]!.Value.Data!.Data).ToArray(), SizePrefix.S32V);
            }
            await SendAsync(packetOut.ToArray());
        }
        public async Task SendPlayChunkFullAsync(int x, int z, ChunkSectionBlocks[] blocks, int[][] biomes, ChunkSectionLight?[] skyLight, ChunkSectionLight?[] blockLight, CompactArray motionBlocking)
        {
            Contract.Requires(State == ProtocolState.Play);
            Contract.Requires(DimensionType.HasValue);
            Contract.Requires(DimensionName is not null);
            Contract.Requires(x < 2097152);
            Contract.Requires(z < 2097152);
            Contract.Requires(x >= -2097152);
            Contract.Requires(z >= -2097152);
            Contract.Requires(blocks.Length * 16 == DimensionType!.Value.Height);
            Contract.Requires(biomes.Length * 16 == DimensionType!.Value.Height);
            Contract.Requires(blockLight.Length * 16 == DimensionType!.Value.Height + 2);
            Contract.Requires(skyLight.Length * 16 == DimensionType!.Value.Height + 2);
            Contract.Requires(motionBlocking.Bits == (int)Math.Log2(DimensionType!.Value.Height));
            Contract.Requires(motionBlocking.Length == 256);
            using MemoryStream packetOut = new();
            packetOut.WriteS32V(0x25);
            packetOut.WriteS32(x);
            packetOut.WriteS32(z);
            packetOut.WriteU8(10);
            packetOut.WriteU8(12);
            packetOut.WriteString("MOTION_BLOCKING", SizePrefix.S16);
            packetOut.WriteS32(motionBlocking.Data.Length);
            packetOut.WriteU64A(motionBlocking.Data);
            packetOut.WriteU8(0);
            using MemoryStream dataOut = new();
            for (int i = 0; i < blocks.Length; i++)
            {
                ChunkSectionBlocks block = blocks[i];
                Dictionary<int, int> blockCounts = [];
                for (int e = 0; e < block.Data.Length; e++)
                {
                    int id = Math.Clamp(block.Data[e], 0, 32768); //TODO: proper auto updating value
                    blockCounts.TryAdd(id, 0);
                    blockCounts[id]++;
                }
                dataOut.WriteU16(block.NotAir);
                byte blockBits = (byte)Math.Log2(blockCounts.Count);
                byte blockBitsTotal = (byte)Math.Log2(32768); //TODO: proper auto updating value
                if (blockBits > 8)
                {
                    CompactArray data = new(blockBitsTotal, block.Data.Length);
                    dataOut.WriteU8(data.Bits);
                    for (int e = 0; e < data.Length; e++) data[e] = Math.Clamp(block.Data[e], 0, 32768); //TODO: proper auto updating value
                    dataOut.WriteU64A(data.Data, SizePrefix.S32V);
                }
                else
                if (blockBits > 0)
                {
                    byte usedBits = blockBits < 4 ? (byte)4 : blockBits;
                    dataOut.WriteU8(usedBits);
                    CompactArray data = new(usedBits, block.Data.Length);
                    Dictionary<int, int> mappings = [];
                    dataOut.WriteS32V(blockCounts.Count);
                    foreach (int id in blockCounts.Keys)
                    {
                        mappings[id] = mappings.Count;
                        dataOut.WriteS32V(id);
                    }
                    for (int e = 0; e < 4096; e++) data[e] = mappings[Math.Clamp(block.Data[e], 0, 32768)]; //TODO: proper auto updating value
                    dataOut.WriteU64A(data.Data, SizePrefix.S32V);
                }
                else
                {
                    dataOut.WriteU8(0);
                    dataOut.WriteS32V(blockCounts.First().Key);
                    dataOut.WriteS32V(0);
                }
                int[] biome = biomes[i];
                Contract.Requires(biome.Length == 64);
                Dictionary<int, int> biomeCounts = [];
                for (int e = 0; e < biome.Length; e++)
                {
                    int id = Math.Clamp(biome[e], 0, Biomes.Count);
                    biomeCounts.TryAdd(id, 0);
                    biomeCounts[id]++;
                }
                byte biomeBits = (byte)Math.Log2(biomeCounts.Count);
                byte biomeBitsTotal = (byte)Math.Log2(Biomes.Count);
                if (biomeBits >= 4)
                {
                    dataOut.WriteU8(biomeBitsTotal);
                    CompactArray data = new(biomeBitsTotal, biome.Length);
                    for (int e = 0; e < data.Length; e++) data[i] = Math.Clamp(biome[e], 0, Biomes.Count);
                    dataOut.WriteU64A(data.Data, SizePrefix.S32V);
                }
                else
                if (biomeBits > 0)
                {
                    byte usedBits = biomeBits < 4 ? (byte)4 : biomeBits;
                    dataOut.WriteU8(usedBits);
                    CompactArray data = new(usedBits, biome.Length);
                    Dictionary<int, int> mappings = [];
                    dataOut.WriteS32V(biomeCounts.Count);
                    foreach (int id in biomeCounts.Keys)
                    {
                        mappings[id] = mappings.Count;
                        dataOut.WriteS32V(id);
                    }
                    for (int e = 0; e < data.Length; e++) data[i] = mappings[Math.Clamp(biome[e], 0, Biomes.Count)];
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
            CompactArray maskSkyLight = new(1, skyLight.Length);
            CompactArray maskSkyLightEmpty = new(1, skyLight.Length);
            CompactArray maskBlockLight = new(1, blockLight.Length);
            CompactArray maskBlockLightEmpty = new(1, blockLight.Length);
            int numSkyLight = 0;
            for (int i = 0; i < skyLight.Length; i++)
            {
                ChunkSectionLight? light = skyLight[i];
                if (light.HasValue)
                {
                    numSkyLight++;
                    maskSkyLight[i] = light.Value.Data is not null ? 1 : 0;
                    maskSkyLightEmpty[i] = light.Value.Data is null ? 1 : 0;
                }
            }
            int numBlockLight = 0;
            for (int i = 0; i < blockLight.Length; i++)
            {
                ChunkSectionLight? light = blockLight[i];
                if (light.HasValue)
                {
                    numBlockLight++;
                    maskBlockLight[i] = light.Value.Data is not null ? 1 : 0;
                    maskBlockLightEmpty[i] = light.Value.Data is null ? 1 : 0;
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
                packetOut.WriteU8A(MemoryMarshal.AsBytes<ulong>(skyLight[i]!.Value.Data!.Data).ToArray(), SizePrefix.S32V);
            }
            packetOut.WriteS32V(numBlockLight);
            for (int i = 0; i < blockLight.Length; i++)
            {
                if (maskBlockLight[i] == 0) continue;
                packetOut.WriteU8A(MemoryMarshal.AsBytes<ulong>(blockLight[i]!.Value.Data!.Data).ToArray(), SizePrefix.S32V);
            }
            await SendAsync(packetOut.ToArray());
        }
        public async Task SendPlayChunkUnloadAsync(int x, int z)
        {
            Contract.Requires(State == ProtocolState.Play);
            Contract.Requires(DimensionName is not null);
            Contract.Requires(x < 2097152);
            Contract.Requires(z < 2097152);
            Contract.Requires(x >= -2097152);
            Contract.Requires(z >= -2097152);
            using MemoryStream packetOut = new();
            packetOut.WriteS32V(0x1F);
            packetOut.WriteS32(x);
            packetOut.WriteS32(z);
            await SendAsync(packetOut.ToArray());
        }
        public async Task SendPlayChunkCenter(int x, int z)
        {
            Contract.Requires(State == ProtocolState.Play);
            Contract.Requires(DimensionName is not null);
            Contract.Requires(x < 2097152);
            Contract.Requires(z < 2097152);
            Contract.Requires(x >= -2097152);
            Contract.Requires(z >= -2097152);
            using MemoryStream packetOut = new();
            packetOut.WriteS32V(0x52);
            packetOut.WriteS32V(x);
            packetOut.WriteS32V(z);
            await SendAsync(packetOut.ToArray());
        }
        public async Task SendPlayChunkWait()
        {
            Contract.Requires(State == ProtocolState.Play);
            Contract.Requires(DimensionName is not null);
            using MemoryStream packetOut = new();
            packetOut.WriteS32V(0x20);
            packetOut.WriteU8(13);
            packetOut.WriteF32(0.0f);
            await SendAsync(packetOut.ToArray());
        }
        public async Task SendPlaySpawnpoint(Position position, float angle)
        {
            Contract.Requires(State == ProtocolState.Play);
            Contract.Requires(DimensionName is not null);
            using MemoryStream packetOut = new();
            packetOut.WriteS32V(0x54);
            packetOut.WriteU64(position.Value);
            packetOut.WriteF32(angle);
            await SendAsync(packetOut.ToArray());
        }
        public async Task SendPlayEntityPosition(int id, double x, double y, double z, byte yaw, byte pitch)
        {
            Contract.Requires(State == ProtocolState.Play);
            Contract.Requires(DimensionName is not null);
            using MemoryStream packetOut = new();
            packetOut.WriteS32V(0x6D);
            packetOut.WriteS32V(id);
            packetOut.WriteF64(x);
            packetOut.WriteF64(y);
            packetOut.WriteF64(z);
            packetOut.WriteU8(yaw);
            packetOut.WriteU8(pitch);
            packetOut.WriteBool(false);
            await SendAsync(packetOut.ToArray());
        }
        public async Task SendPlayPlayerPosition(double x, double y, double z, float yaw, float pitch, int id)
        {
            Contract.Requires(State == ProtocolState.Play);
            Contract.Requires(DimensionName is not null);
            using MemoryStream packetOut = new();
            packetOut.WriteS32V(0x3E);
            packetOut.WriteF64(x);
            packetOut.WriteF64(y);
            packetOut.WriteF64(z);
            packetOut.WriteF32(yaw);
            packetOut.WriteF32(pitch);
            packetOut.WriteU8(0);
            packetOut.WriteS32V(id);
            await SendAsync(packetOut.ToArray());
        }
        private async Task ListenAsync()
        {
            //try
            {
                while (true) //TODO: disconnect
                {
                    PacketQueue.Enqueue(await ReceiveAsync());
                }
            }
            //catch(Exception ex)
            {
                //TODO: throw error
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

        [GeneratedRegex("^[a-zA-Z0-9_]{1,16}$")]
        private static partial Regex MinecraftUsernameRegex();

        [GeneratedRegex("^[a-zA-Z0-9_]+:[a-zA-Z0-9_]+$")]
        internal static partial Regex NamespaceRegex();
    }
}
