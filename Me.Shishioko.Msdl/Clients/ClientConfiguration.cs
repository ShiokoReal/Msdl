using Me.Shishioko.Msdl.Connections;
using Me.Shishioko.Msdl.Data;
using Me.Shishioko.Msdl.Data.Chat;
using Me.Shishioko.Msdl.Data.Entities;
using Me.Shishioko.Msdl.Data.Protocol;
using Net.Myzuc.ShioLib;
using System;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Me.Shishioko.Msdl.Clients
{
    public sealed class ClientConfiguration
    {
        private readonly Client Client;
        public Func<Preferences, Task> ReceivePreferences = (Preferences preferences) => Task.CompletedTask;
        public Func<string, byte[]?, Task> ReceiveCookieResponse = (string identifier, byte[]? Data) => Task.CompletedTask;
        public Func<string, byte[], Task> ReceiveCustom = (string channel, byte[] data) => Task.CompletedTask;
        public Func<long, Task> ReceiveHeartbeat = (long sequence) => Task.CompletedTask;
        public Func<int, Task> ReceivePingResponse = (int sequence) => Task.CompletedTask;
        public Func<ClientPlay, Task> SwitchPlay = (ClientPlay client) => Task.CompletedTask;
        private bool Complete = false;
        private Dimension[]? DimensionTypes = null;
        private Biome[]? Biomes = null;
        internal ClientConfiguration(Client client)
        {
            Client = client;
        }
        public async Task StartReceivingAsync()
        {
            while (true)
            {
                using MemoryStream packetIn = new(await Client.ReceiveAsync());
                switch (packetIn.ReadS32V())
                {
                    case Packets.IncomingConfigurationInformation:
                        {
                            string language = packetIn.ReadString(SizePrefix.S32V, 16);
                            byte renderDistance = packetIn.ReadU8();
                            ChatMode chatMode = (ChatMode)packetIn.ReadS32V();
                            bool chatColors = packetIn.ReadBool();
                            EntityPlayer.EntityPlayerSkinMask skinMask = new(packetIn.ReadU8());
                            bool rightHanded = packetIn.ReadS32V() == 0 ? false : true;
                            packetIn.ReadBool();
                            bool listing = packetIn.ReadBool();
                            await ReceivePreferences(new(language, renderDistance, chatMode, chatColors, skinMask, rightHanded, listing));
                            break;
                        }
                    case Packets.IncomingConfigurationCookieResponse:
                        {
                            string identifier = packetIn.ReadString(SizePrefix.S32V, 32767);
                            byte[]? data = packetIn.ReadBool() ? packetIn.ReadU8A(SizePrefix.S32V, 5120) : null;
                            await ReceiveCookieResponse(identifier, data);
                            break;
                        }
                    case Packets.IncomingConfigurationPluginMessage:
                        {
                            string channel = packetIn.ReadString(SizePrefix.S32V, 32767);
                            byte[] data = packetIn.ReadU8A((int)(packetIn.Length - packetIn.Position));
                            await ReceiveCustom(channel, data);
                            break;
                        }
                    case Packets.IncomingConfigurationEnd:
                        {
                            if (!Complete) throw new ProtocolViolationException();
                            Complete = true;
                            await SwitchPlay(new ClientPlay(Client, DimensionTypes!, Biomes!));
                            break;
                        }
                    case Packets.IncomingConfigurationHeartbeat:
                        {
                            await ReceiveHeartbeat(packetIn.ReadS64());
                            break;
                        }
                    case Packets.IncomingConfigurationPingResponse:
                        {
                            await ReceivePingResponse(packetIn.ReadS32());
                            break;
                        }
                    case Packets.IncomingConfigurationResourcpackAdd:
                        {
                            //TODO: implement incoming config resourcepack response
                            break;
                        }
                    default:
                        {
                            throw new ProtocolViolationException();
                        }
                }
            }
        }
        public Task SendCookieRequestAsync(string identifier)
        {
            if (Complete) throw new InvalidOperationException();
            using MemoryStream packetOut = new();
            packetOut.WriteS32V(Packets.OutgoingConfigurationCookieRequest);
            packetOut.WriteString(identifier, SizePrefix.S32V, 32767);
            return Client.SendAsync(packetOut.ToArray());
        }
        public Task SendCustomAsync(string channel, byte[] data, int sequence)
        {
            if (Complete) throw new InvalidOperationException();
            using MemoryStream packetOut = new();
            packetOut.WriteS32V(Packets.OutgoingConfigurationPluginMessage);
            packetOut.WriteString(channel, SizePrefix.S32V, 1048576);
            packetOut.WriteU8A(data);
            return Client.SendAsync(packetOut.ToArray());
        }
        public async Task SendDisconnectAsync(ChatComponent message)
        {
            if (Complete) throw new InvalidOperationException();
            using MemoryStream packetOut = new();
            packetOut.WriteS32V(Packets.OutgoingConfigurationDisconnect);
            packetOut.WriteU8(0x0A);
            message.Serialize(packetOut);
            await Client.SendAsync(packetOut.ToArray());
            Client.Dispose();
        }
        public Task SendEndAsync()
        {
            if (Complete) throw new InvalidOperationException();
            if (DimensionTypes is null || Biomes is null) throw new InvalidOperationException();
            Complete = true;
            using MemoryStream packetOut = new();
            packetOut.WriteS32V(Packets.OutgoingConfigurationEnd);
            return Client.SendAsync(packetOut.ToArray());
        }
        public Task SendHeartbeatAsync(long sequence)
        {
            if (Complete) throw new InvalidOperationException();
            using MemoryStream packetOut = new();
            packetOut.WriteS32V(Packets.OutgoingConfigurationHeartbeat);
            packetOut.WriteS64(sequence);
            return Client.SendAsync(packetOut.ToArray());
        }
        public Task SendPingAsync(int sequence)
        {
            if (Complete) throw new InvalidOperationException();
            using MemoryStream packetOut = new();
            packetOut.WriteS32V(Packets.OutgoingConfigurationPingRequest);
            packetOut.WriteS32(sequence);
            return Client.SendAsync(packetOut.ToArray());
        }
        public Task SendChatClearAsync()
        {
            if (Complete) throw new InvalidOperationException();
            using MemoryStream packetOut = new();
            packetOut.WriteS32V(Packets.OutgoingConfigurationChatClear);
            return Client.SendAsync(packetOut.ToArray());
        }
        public Task SendRegistryAsync(Dimension[] dimensionTypes)
        {
            if (Complete) throw new InvalidOperationException();
            if (dimensionTypes.Length <= 0) throw new ArgumentException();
            DimensionTypes = dimensionTypes;
            using MemoryStream packetOut = new();
            packetOut.WriteS32V(Packets.OutgoingConfigurationRegistry);
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
            return Client.SendAsync(packetOut.ToArray());
        }
        public Task SendRegistryAsync(Biome[] biomes)
        {
            if (Complete) throw new InvalidOperationException();
            if (biomes.Length <= 0) throw new ArgumentException();
            if (!biomes.Any(biome => biome.Name == "minecraft:plains")) throw new ArgumentException();
            Biomes = biomes;
            using MemoryStream packetOut = new();
            packetOut.WriteS32V(Packets.OutgoingConfigurationRegistry);
            packetOut.WriteString("minecraft:worldgen/biome", SizePrefix.S32V);
            packetOut.WriteS32V(Biomes.Length);
            for (int i = 0; i < Biomes.Length; i++)
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
            return Client.SendAsync(packetOut.ToArray());
        }
        //TODO: texture pack remove
        //TODO: texture pack add
        public async Task SendCookieAsync(string identifier, byte[] data)
        {
            if (Complete) throw new InvalidOperationException();
            using MemoryStream packetOut = new();
            packetOut.WriteS32V(Packets.OutgoingConfigurationCookieStore);
            packetOut.WriteString(identifier, SizePrefix.S32V, 32767);
            packetOut.WriteU8A(data, SizePrefix.S32V, 5120); //TODO: test if length is realy not specified
            await Client.SendAsync(packetOut.ToArray());
            Client.Dispose();
        }
        public async Task SendTransferAsync(string endpoint, ushort port)
        {
            if (Complete) throw new InvalidOperationException();
            using MemoryStream packetOut = new();
            packetOut.WriteS32V(Packets.OutgoingConfigurationTransfer);
            packetOut.WriteString(endpoint, SizePrefix.S32V, 32767);
            packetOut.WriteU16(port);
            await Client.SendAsync(packetOut.ToArray());
            Client.Dispose();
        }
        //TODO: feature flags
        //TODO: tags
        public async Task TEMP_A_SendRegistryAsync()
        {
            if (Complete) throw new InvalidOperationException();
            {
                using MemoryStream packetOut = new();
                packetOut.WriteS32V(Packets.OutgoingConfigurationRegistry);
                packetOut.WriteString("minecraft:trim_pattern", SizePrefix.S32V);
                packetOut.WriteS32V(0);
                await Client.SendAsync(packetOut.ToArray());
            }
            {
                using MemoryStream packetOut = new();
                packetOut.WriteS32V(Packets.OutgoingConfigurationRegistry);
                packetOut.WriteString("minecraft:trim_material", SizePrefix.S32V);
                packetOut.WriteS32V(0);
                await Client.SendAsync(packetOut.ToArray());
            }
            {
                using MemoryStream packetOut = new();
                packetOut.WriteS32V(Packets.OutgoingConfigurationRegistry);
                packetOut.WriteString("minecraft:chat_type", SizePrefix.S32V);
                packetOut.WriteS32V(0);
                await Client.SendAsync(packetOut.ToArray());
            }
            {
                using MemoryStream packetOut = new();
                packetOut.WriteS32V(Packets.OutgoingConfigurationRegistry);
                packetOut.WriteString("minecraft:worldgen/biome", SizePrefix.S32V);
                packetOut.WriteS32V(0);
                await Client.SendAsync(packetOut.ToArray());
            }
            {
                using MemoryStream packetOut = new();
                packetOut.WriteS32V(Packets.OutgoingConfigurationRegistry);
                packetOut.WriteString("minecraft:wolf_variant", SizePrefix.S32V);
                packetOut.WriteS32V(0);
                await Client.SendAsync(packetOut.ToArray());
            }
            {
                using MemoryStream packetOut = new();
                packetOut.WriteS32V(Packets.OutgoingConfigurationRegistry);
                packetOut.WriteString("minecraft:banner_pattern", SizePrefix.S32V);
                packetOut.WriteS32V(0);
                await Client.SendAsync(packetOut.ToArray());
            }
        }
        public Task TEMP_B_SendRegistryAsync() //TODO: other registries
        {
            if (Complete) throw new InvalidOperationException();
            using MemoryStream packetOut = new();
            packetOut.WriteS32V(Packets.OutgoingConfigurationRegistry);
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
            "minecraft:thrown",
            "minecraft:spit",
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
            return Client.SendAsync(packetOut.ToArray());
        }
        public Task SendFluidsAsync(int[] water, int[] lava) //TODO: fluid ids from liquid class
        {
            if (Complete) throw new InvalidOperationException();
            using MemoryStream packetOut = new();
            packetOut.WriteS32V(Packets.OutgoingConfigurationTags);
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
            return Client.SendAsync(packetOut.ToArray());
        }

    }
}
