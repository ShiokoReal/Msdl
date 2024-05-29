using Me.Shishioko.Msdl.Content;
using Me.Shishioko.Msdl.Data;
using Me.Shishioko.Msdl.Util;
using Microsoft.VisualStudio.Threading;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using static Me.Shishioko.Msdl.Content.Chunk;
using static System.Collections.Specialized.BitVector32;

namespace Me.Shishioko.Msdl
{
    public sealed class Client
    { //TODO: set proper access
        internal readonly Connection Connection;
        public readonly Guid Guid;
        public readonly string Name;
        public readonly IEnumerable<Property> Properties;
        public event Action<string, byte[]> OnPluginMessage = (string channel, byte[] data) => { };
        internal Dimension[]? DimensionTypes = null;
        internal Dimension? DimensionType = null;
        internal string[]? DimensionNames = null;
        internal string? DimensionName = null;
        internal bool Configuring = true;
        internal IReadOnlyList<Biome> Biomes = [new Biome("minecraft:plains")];
        //damage types?
        internal bool Hardcore = false;
        internal bool RespawnScreen = false;


        internal Client(Connection connection, Guid guid, string name, IEnumerable<Property> properties)
        {
            Connection = connection;
            Guid = guid;
            Name = name;
            Properties = properties;
        } //TODO: merge with connection.cs
        public void SendConfigurationRegistries(Dimension[] dimensionTypes, Biome[] biomes) //TODO: other registries
        {
            Contract.Requires(Configuring);
            Contract.Requires(dimensionTypes.Length > 0);
            Contract.Requires(biomes.Length > 0);
            DimensionTypes = dimensionTypes;
            Biomes = biomes;
            using DataStream packetOut = new(new MemoryStream());
            packetOut.WriteS32V(0x05);
            packetOut.WriteU8(10);
            packetOut.WriteU8(10);
            packetOut.WriteStringS16("minecraft:damage_type");
            packetOut.WriteU8(8);
            packetOut.WriteStringS16("type");
            packetOut.WriteStringS16("minecraft:damage_type");
            packetOut.WriteU8(9);
            packetOut.WriteStringS16("value");
            packetOut.WriteU8(10);
            string[] damages = {
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
            };
            packetOut.WriteS32(damages.Length);
            for (int i = 0; i < damages.Length; i++)
            {
                packetOut.WriteU8(8);
                packetOut.WriteStringS16("name");
                packetOut.WriteStringS16(damages[i]);
                packetOut.WriteU8(3);
                packetOut.WriteStringS16("id");
                packetOut.WriteS32(i);
                packetOut.WriteU8(10);
                packetOut.WriteStringS16("element");
                packetOut.WriteU8(5);
                packetOut.WriteStringS16("exhaustion");
                packetOut.WriteF32(0.0f);
                packetOut.WriteU8(8);
                packetOut.WriteStringS16("scaling");
                packetOut.WriteStringS16("always");
                packetOut.WriteU8(8);
                packetOut.WriteStringS16("message_id");
                packetOut.WriteStringS16("arrow");
                packetOut.WriteU8(0);
                packetOut.WriteU8(0);
            }
            packetOut.WriteU8(0);
            packetOut.WriteU8(10);
            packetOut.WriteStringS16("minecraft:dimension_type");
            packetOut.WriteU8(8);
            packetOut.WriteStringS16("type");
            packetOut.WriteStringS16("minecraft:dimension_type");
            packetOut.WriteU8(9);
            packetOut.WriteStringS16("value");
            packetOut.WriteU8(10);
            packetOut.WriteS32(DimensionTypes.Length);
            for (int i = 0; i < dimensionTypes.Length; i++)
            {
                Dimension dimensionType = DimensionTypes[i];
                packetOut.WriteU8(8);
                packetOut.WriteStringS16("name");
                packetOut.WriteStringS16(dimensionType.Name);
                packetOut.WriteU8(3);
                packetOut.WriteStringS16("id");
                packetOut.WriteS32(i);
                packetOut.WriteU8(10);
                packetOut.WriteStringS16("element");
                packetOut.WriteU8(5);
                packetOut.WriteStringS16("ambient_light");
                packetOut.WriteF32(dimensionType.AmbientLight);
                packetOut.WriteU8(1);
                packetOut.WriteStringS16("bed_works");
                packetOut.WriteBool(true);
                packetOut.WriteU8(6);
                packetOut.WriteStringS16("coordinate_scale");
                packetOut.WriteF64(1.0);
                packetOut.WriteU8(8);
                packetOut.WriteStringS16("effects");
                packetOut.WriteStringS16(
                    dimensionType.Effects == Sky.Overworld ? "minecraft:overworld" : //TODO: no enum
                    dimensionType.Effects == Sky.Nether ? "minecraft:the_nether" :
                    dimensionType.Effects == Sky.End ? "minecraft:the_end" :
                    "minecraft:overworld"
                    );
                packetOut.WriteU8(1);
                packetOut.WriteStringS16("has_ceiling");
                packetOut.WriteBool(false);
                packetOut.WriteU8(1);
                packetOut.WriteStringS16("has_raids");
                packetOut.WriteBool(true);
                packetOut.WriteU8(1);
                packetOut.WriteStringS16("has_skylight");
                packetOut.WriteBool(dimensionType.HasSkylight);
                packetOut.WriteU8(3);
                packetOut.WriteStringS16("height");
                packetOut.WriteS32(dimensionType.Height);
                packetOut.WriteU8(8);
                packetOut.WriteStringS16("infiniburn");
                packetOut.WriteStringS16("#minecraft:infiniburn_overworld");
                packetOut.WriteU8(3);
                packetOut.WriteStringS16("logical_height");
                packetOut.WriteS32(dimensionType.Height);
                packetOut.WriteU8(3);
                packetOut.WriteStringS16("min_y");
                packetOut.WriteS32(dimensionType.Depth);
                packetOut.WriteU8(3);
                packetOut.WriteStringS16("monster_spawn_block_light_limit");
                packetOut.WriteS32(0);
                packetOut.WriteU8(3);
                packetOut.WriteStringS16("monster_spawn_light_level");
                packetOut.WriteS32(0);
                packetOut.WriteU8(1);
                packetOut.WriteStringS16("natural");
                packetOut.WriteBool(dimensionType.Natural);
                packetOut.WriteU8(1);
                packetOut.WriteStringS16("piglin_safe");
                packetOut.WriteBool(false);
                packetOut.WriteU8(1);
                packetOut.WriteStringS16("respawn_anchor_works");
                packetOut.WriteBool(true);
                packetOut.WriteU8(1);
                packetOut.WriteStringS16("ultrawarm");
                packetOut.WriteBool(false);
                packetOut.WriteU8(0);
                packetOut.WriteU8(0);
            }
            packetOut.WriteU8(0);
            packetOut.WriteU8(10);
            packetOut.WriteStringS16("minecraft:worldgen/biome");
            packetOut.WriteU8(8);
            packetOut.WriteStringS16("type");
            packetOut.WriteStringS16("minecraft:worldgen/biome");
            packetOut.WriteU8(9);
            packetOut.WriteStringS16("value");
            packetOut.WriteU8(10);
            packetOut.WriteS32(Biomes.Count);
            uint bid = 0;
            foreach (Biome biome in Biomes)
            {
                packetOut.WriteU8(8);
                packetOut.WriteStringS16("name");
                packetOut.WriteStringS16(biome.Name);
                packetOut.WriteU8(3);
                packetOut.WriteStringS16("id");
                packetOut.WriteU32(bid++);
                packetOut.WriteU8(10);
                packetOut.WriteStringS16("element");
                packetOut.WriteU8(1);
                packetOut.WriteStringS16("has_precipitation");
                packetOut.WriteBool(biome.Precipitation);
                packetOut.WriteU8(5);
                packetOut.WriteStringS16("temperature");
                packetOut.WriteF32(0.0f);
                packetOut.WriteU8(5);
                packetOut.WriteStringS16("downfall");
                packetOut.WriteF32(0.0f);
                packetOut.WriteU8(10);
                packetOut.WriteStringS16("effects");
                packetOut.WriteU8(3);
                packetOut.WriteStringS16("sky_color");
                packetOut.WriteU8(biome.SkyColor.A);
                packetOut.WriteU8(biome.SkyColor.R);
                packetOut.WriteU8(biome.SkyColor.G);
                packetOut.WriteU8(biome.SkyColor.B);
                packetOut.WriteU8(3);
                packetOut.WriteStringS16("water_fog_color");
                packetOut.WriteU8(biome.WaterFogColor.A);
                packetOut.WriteU8(biome.WaterFogColor.R);
                packetOut.WriteU8(biome.WaterFogColor.G);
                packetOut.WriteU8(biome.WaterFogColor.B);
                packetOut.WriteU8(3);
                packetOut.WriteStringS16("fog_color");
                packetOut.WriteU8(biome.FogColor.A);
                packetOut.WriteU8(biome.FogColor.R);
                packetOut.WriteU8(biome.FogColor.G);
                packetOut.WriteU8(biome.FogColor.B);
                packetOut.WriteU8(3);
                packetOut.WriteStringS16("water_color");
                packetOut.WriteU8(biome.WaterColor.A);
                packetOut.WriteU8(biome.WaterColor.R);
                packetOut.WriteU8(biome.WaterColor.G);
                packetOut.WriteU8(biome.WaterColor.B);
                if (biome.FoliageColor.HasValue)
                {
                    packetOut.WriteU8(3);
                    packetOut.WriteStringS16("foliage_color");
                    packetOut.WriteU8(biome.FoliageColor.Value.A);
                    packetOut.WriteU8(biome.FoliageColor.Value.R);
                    packetOut.WriteU8(biome.FoliageColor.Value.G);
                    packetOut.WriteU8(biome.FoliageColor.Value.B);
                }
                if (biome.GrassColor.HasValue)
                {
                    packetOut.WriteU8(3);
                    packetOut.WriteStringS16("grass_color");
                    packetOut.WriteU8(biome.GrassColor.Value.A);
                    packetOut.WriteU8(biome.GrassColor.Value.R);
                    packetOut.WriteU8(biome.GrassColor.Value.G);
                    packetOut.WriteU8(biome.GrassColor.Value.B);
                }
                if (biome.Music.HasValue)
                {
                    packetOut.WriteU8(10);
                    packetOut.WriteStringS16("music");
                    packetOut.WriteU8(1);
                    packetOut.WriteStringS16("replace_current_music");
                    packetOut.WriteBool(biome.Music.Value.ReplaceCurrentMusic);
                    packetOut.WriteU8(8);
                    packetOut.WriteStringS16("sound");
                    packetOut.WriteStringS16(biome.Music.Value.Sound);
                    packetOut.WriteU8(3);
                    packetOut.WriteStringS16("max_delay");
                    packetOut.WriteS32((int)(biome.Music.Value.MaxDelay.TotalSeconds * 20.0d));
                    packetOut.WriteU8(3);
                    packetOut.WriteStringS16("min_delay");
                    packetOut.WriteS32((int)(biome.Music.Value.MinDelay.TotalSeconds * 20.0d));
                    packetOut.WriteU8(0);
                }
                if (biome.AmbientSound is not null)
                {
                    packetOut.WriteU8(8);
                    packetOut.WriteStringS16("ambient_sound");
                    packetOut.WriteStringS16(biome.AmbientSound);
                }
                if (biome.AdditionsSound.HasValue)
                {
                    packetOut.WriteU8(10);
                    packetOut.WriteStringS16("additions_sound");
                    packetOut.WriteU8(8);
                    packetOut.WriteStringS16("sound");
                    packetOut.WriteStringS16(biome.AdditionsSound.Value.Sound);
                    packetOut.WriteU8(6);
                    packetOut.WriteStringS16("tick_chance");
                    packetOut.WriteF64(biome.AdditionsSound.Value.Chance / 20.0d);
                    packetOut.WriteU8(0);
                }
                if (biome.MoodSound.HasValue)
                {
                    packetOut.WriteU8(10);
                    packetOut.WriteStringS16("additions_sound");
                    packetOut.WriteU8(8);
                    packetOut.WriteStringS16("sound");
                    packetOut.WriteStringS16(biome.MoodSound.Value.Sound);
                    packetOut.WriteU8(3);
                    packetOut.WriteStringS16("tick_delay");
                    packetOut.WriteS32((int)(biome.MoodSound.Value.Delay.TotalSeconds * 20.0d));
                    packetOut.WriteU8(6);
                    packetOut.WriteStringS16("offset");
                    packetOut.WriteF64(biome.MoodSound.Value.Offset);
                    packetOut.WriteU8(3);
                    packetOut.WriteStringS16("block_search_extent");
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
            Connection.Send(packetOut.Get());
            /*using (DataStream packetOut = new(new MemoryStream()))
            {
                packetOut.WriteS32V(0x02);
                Connection.Send(packetOut.Get());
            }
            while (!Configuring.IsSet)
            {
                using DataStream packetIn = new(Connection.Receive());
                switch (packetIn.ReadS32V())
                {
                    case 0x00:
                        {
                            break; //TODO:
                        }
                    case 0x01:
                        {
                            string channel = packetIn.ReadStringS32V();
                            if (!Connection.NamespaceRegex().IsMatch(channel)) throw new ProtocolViolationException();
                            MemoryStream baseStream = (MemoryStream)packetIn.Stream;
                            byte[] data = packetIn.ReadU8A((int)(baseStream.Length - baseStream.Position));
                            OnPluginMessage(channel, data);
                            break;
                        }
                    case 0x02:
                        {
                            Configuring.Set();
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
            Update();
            while (true)
            {
                using DataStream packetIn = new(Connection.Receive());
                switch (packetIn.ReadS32V())
                {
                    default:
                        {
                            //throw new ProtocolViolationException(); //TODO: remove comment
                            break;
                        }
                }
            }*/
        }
        public void SendLogin(int entityID, bool hardcore, int viewDistance, int simulationDistance, bool reducedDebug, bool respawnScreen, Dimension dimensionType, string dimensionName, ulong seedHash, Gamemode currentGamemode, Gamemode? previousGamemode, bool flatWorld, DeathLocation? deathLocation)
        {
            Contract.Requires(!Configuring);
            Contract.Requires(DimensionTypes is not null);
            Contract.Requires(DimensionNames is not null);
            Contract.Requires(viewDistance >= 0);
            Contract.Requires(simulationDistance >= 0);
            Contract.Requires(DimensionTypes!.Contains(dimensionType));
            Contract.Requires(DimensionNames!.Contains(dimensionName));
            using DataStream packetOut = new(new MemoryStream());
            packetOut.WriteS32V(0x29);
            packetOut.WriteS32(entityID);
            packetOut.WriteBool(Hardcore = hardcore);
            packetOut.WriteS32V(DimensionNames!.Length);
            for (int i = 0; i < DimensionNames.Length; i++) packetOut.WriteStringS32V(DimensionNames[i]);
            packetOut.WriteS32V(0);
            packetOut.WriteS32V(viewDistance);
            packetOut.WriteS32V(simulationDistance);
            packetOut.WriteBool(reducedDebug);
            packetOut.WriteBool(RespawnScreen = respawnScreen);
            packetOut.WriteBool(false);
            packetOut.WriteStringS32V((DimensionType = dimensionType).Value.Name);
            packetOut.WriteStringS32V(DimensionName = dimensionName);
            packetOut.WriteU64(seedHash);
            packetOut.WriteU8((byte)currentGamemode);
            packetOut.WriteS8(previousGamemode.HasValue ? (sbyte)previousGamemode.Value : (sbyte)-1);
            packetOut.WriteBool(false);
            packetOut.WriteBool(flatWorld);
            packetOut.WriteBool(deathLocation.HasValue);
            if (deathLocation.HasValue)
            {
                Contract.Requires(DimensionNames.Contains(deathLocation.Value.Dimension));
                packetOut.WriteStringS32V(deathLocation.Value.Dimension);
                packetOut.WriteU64(deathLocation.Value.Location.Value);
            }
            packetOut.WriteS32V(0);
            Connection.Send(packetOut.Get());
        }
        public void SendChunkBiomes(int x, int z, int[][] biomes)
        {
            Contract.Requires(!Configuring);
            Contract.Requires(DimensionType.HasValue);
            Contract.Requires(DimensionName is not null);
            Contract.Requires(x < 2097152);
            Contract.Requires(z < 2097152);
            Contract.Requires(x >= -2097152);
            Contract.Requires(z >= -2097152);
            Contract.Requires(biomes.Length * 16 == DimensionType!.Value.Height);
            using DataStream packetOut = new();
            packetOut.WriteS32V(0x0E);
            packetOut.WriteS32V(1);
            packetOut.WriteS32(x);
            packetOut.WriteS32(z);
            using DataStream dataOut = new();
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
                byte bits = (byte)(Math.Log2(counts.Count) + 1.0);
                byte totalbits = (byte)(Math.Log2(Biomes.Count) + 1.0);
                if (bits >= 4)
                {
                    dataOut.WriteU8(totalbits);
                    CompactArray data = new(totalbits, biome.Length);
                    for (int e = 0; e < data.Length; e++) data[i] = Math.Clamp(biome[e], 0, Biomes.Count);
                    dataOut.WriteU64AV(data.Data);
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
                    dataOut.WriteU64AV(data.Data);
                }
                else
                {
                    dataOut.WriteU8(0);
                    dataOut.WriteS32V(counts.First().Key);
                    dataOut.WriteS32V(0);
                }
            }
            packetOut.WriteU8AV(dataOut.Get());
            Connection.Send(packetOut.Get());
        }
        public void SendChunkLight(int x, int z, ChunkSectionLight?[] skyLight, ChunkSectionLight?[] blockLight)
        {
            Contract.Requires(!Configuring);
            Contract.Requires(DimensionType.HasValue);
            Contract.Requires(DimensionName is not null);
            Contract.Requires(x < 2097152);
            Contract.Requires(z < 2097152);
            Contract.Requires(x >= -2097152);
            Contract.Requires(z >= -2097152);
            Contract.Requires(blockLight.Length * 16 == DimensionType!.Value.Height + 2);
            Contract.Requires(skyLight.Length * 16 == DimensionType!.Value.Height + 2);
            using DataStream packetOut = new();
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
            packetOut.WriteU64AV(maskSkyLight.Data);
            packetOut.WriteU64AV(maskBlockLight.Data);
            packetOut.WriteU64AV(maskSkyLightEmpty.Data);
            packetOut.WriteU64AV(maskBlockLightEmpty.Data);
            packetOut.WriteS32V(numSkyLight);
            for (int i = 0; i < skyLight.Length; i++)
            {
                if (maskSkyLight[i] == 0) continue;
                packetOut.WriteU8AV(MemoryMarshal.AsBytes<ulong>(skyLight[i]!.Value.Data!.Data).ToArray());
            }
            packetOut.WriteS32V(numBlockLight);
            for (int i = 0; i < blockLight.Length; i++)
            {
                if (maskBlockLight[i] == 0) continue;
                packetOut.WriteU8AV(MemoryMarshal.AsBytes<ulong>(blockLight[i]!.Value.Data!.Data).ToArray());
            }
            Connection.Send(packetOut.Get());
        }
        public void SendChunkFull(int x, int z, ChunkSectionBlocks[] blocks, int[][] biomes, ChunkSectionLight?[] skyLight, ChunkSectionLight?[] blockLight, CompactArray motionBlocking)
        {
            Contract.Requires(!Configuring);
            Contract.Requires(!DimensionType.HasValue);
            Contract.Requires(DimensionName is not null);
            Contract.Requires(x < 2097152);
            Contract.Requires(z < 2097152);
            Contract.Requires(x >= -2097152);
            Contract.Requires(z >= -2097152);
            Contract.Requires(blocks.Length * 16 == DimensionType!.Value.Height);
            Contract.Requires(biomes.Length * 16 == DimensionType!.Value.Height);
            Contract.Requires(blockLight.Length * 16 == DimensionType!.Value.Height + 2);
            Contract.Requires(skyLight.Length * 16 == DimensionType!.Value.Height + 2);
            Contract.Requires(motionBlocking.Bits == (int)(Math.Log2(DimensionType!.Value.Height) + 1));
            Contract.Requires(motionBlocking.Length == 256);
            using DataStream packetOut = new();
            packetOut.WriteS32V(0x25);
            packetOut.WriteS32(x);
            packetOut.WriteS32(z);
            packetOut.WriteU8(10);
            packetOut.WriteStringS16(string.Empty);
            packetOut.WriteU8(12);
            packetOut.WriteStringS16("MOTION_BLOCKING");
            packetOut.WriteS32(motionBlocking.Data.Length);
            packetOut.WriteU64A(motionBlocking.Data);
            packetOut.WriteU8(0);
            using DataStream dataOut = new();
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
                byte blockBits = (byte)(Math.Log2(blockCounts.Count) + 1.0);
                byte blockBitsTotal = (byte)(Math.Log2(32768) + 1.0); //TODO: proper auto updating value
                if (blockBits > 8)
                {
                    CompactArray data = new(blockBitsTotal, block.Data.Length);
                    dataOut.WriteU8(data.Bits);
                    for (int e = 0; e < data.Length; e++) data[e] = Math.Clamp(block.Data[e], 0, 32768); //TODO: proper auto updating value
                    dataOut.WriteU64AV(data.Data);
                }
                else
                if (blockBits > 0)
                {
                    dataOut.WriteU8(blockBits < 4 ? (byte)4 : blockBits);
                    CompactArray data = new(blockBits < 4 ? (byte)4 : blockBits, block.Data.Length);
                    Dictionary<int, int> mappings = [];
                    dataOut.WriteS32V(blockCounts.Count);
                    foreach (ushort id in blockCounts.Keys)
                    {
                        mappings[id] = mappings.Count;
                        dataOut.WriteS32V(id);
                    }
                    for (int e = 0; e < 4096; e++) data[e] = mappings[Math.Clamp(block.Data[e], 0, 32768)]; //TODO: proper auto updating value
                    dataOut.WriteU64AV(data.Data);
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
                byte biomeBits = (byte)(Math.Log2(biomeCounts.Count) + 1.0);
                byte biomeBitsTotal = (byte)(Math.Log2(Biomes.Count) + 1.0);
                if (biomeBits >= 4)
                {
                    dataOut.WriteU8(biomeBitsTotal);
                    CompactArray data = new(biomeBitsTotal, biome.Length);
                    for (int e = 0; e < data.Length; e++) data[i] = Math.Clamp(biome[e], 0, Biomes.Count);
                    dataOut.WriteU64AV(data.Data);
                }
                else
                if (biomeBits > 0)
                {
                    dataOut.WriteU8(biomeBits);
                    CompactArray data = new(biomeBits < 4 ? (byte)4 : biomeBits, biome.Length);
                    Dictionary<int, int> mappings = [];
                    dataOut.WriteS32V(biomeCounts.Count);
                    foreach (int id in biomeCounts.Keys)
                    {
                        mappings[id] = mappings.Count;
                        dataOut.WriteS32V(id);
                    }
                    for (int e = 0; e < data.Length; e++) data[i] = mappings[Math.Clamp(biome[e], 0, Biomes.Count)];
                    dataOut.WriteU64AV(data.Data);
                }
                else
                {
                    dataOut.WriteU8(0);
                    dataOut.WriteS32V(biomeCounts.First().Key);
                    dataOut.WriteS32V(0);
                }
            }
            packetOut.WriteU8AV(dataOut.Get());
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
            packetOut.WriteU64AV(maskSkyLight.Data);
            packetOut.WriteU64AV(maskBlockLight.Data);
            packetOut.WriteU64AV(maskSkyLightEmpty.Data);
            packetOut.WriteU64AV(maskBlockLightEmpty.Data);
            packetOut.WriteS32V(numSkyLight);
            for (int i = 0; i < skyLight.Length; i++)
            {
                if (maskSkyLight[i] == 0) continue;
                packetOut.WriteU8AV(MemoryMarshal.AsBytes<ulong>(skyLight[i]!.Value.Data!.Data).ToArray());
            }
            packetOut.WriteS32V(numBlockLight);
            for (int i = 0; i < blockLight.Length; i++)
            {
                if (maskBlockLight[i] == 0) continue;
                packetOut.WriteU8AV(MemoryMarshal.AsBytes<ulong>(blockLight[i]!.Value.Data!.Data).ToArray());
            }
            Connection.Send(packetOut.Get());
        }
        public void SendChunkUnload(int x, int z)
        {
            Contract.Requires(!Configuring);
            Contract.Requires(DimensionName is not null);
            Contract.Requires(x < 2097152);
            Contract.Requires(z < 2097152);
            Contract.Requires(x >= -2097152);
            Contract.Requires(z >= -2097152);
            using DataStream packetOut = new();
            packetOut.WriteS32V(0x1F);
            packetOut.WriteS32(x);
            packetOut.WriteS32(z);
            Connection.Send(packetOut.Get());
        }
    }
}
