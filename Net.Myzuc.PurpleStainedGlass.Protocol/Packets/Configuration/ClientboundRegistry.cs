using Net.Myzuc.PurpleStainedGlass.Protocol.Objects;
using Net.Myzuc.PurpleStainedGlass.Protocol.Const;
using Net.Myzuc.ShioLib;
using System.IO;

namespace Net.Myzuc.PurpleStainedGlass.Protocol.Packets.Configuration
{
    public sealed class ClientboundRegistry : Clientbound
    {
        private const int Id = 0x07;
        public override Client.EnumState Mode => Client.EnumState.Configuration;
        private readonly RegistryDimension[]? DimensionRegistry = null;
        private readonly RegistryBiome[]? BiomeRegistry = null;
        private readonly RegistryChat[]? ChatRegistry = null;
        private readonly RegistryDamage[]? DamageRegistry = null;
        public ClientboundRegistry(RegistryDimension[] registry)
        {
            using MemoryStream stream = new();
            stream.WriteS32V(Id);
            stream.WriteString("minecraft:dimension_type", SizePrefix.S32V);
            stream.WriteS32V(registry.Length);
            for (int i = 0; i < registry.Length; i++)
            {
                RegistryDimension entry = registry[i];
                stream.WriteString(entry.Name, SizePrefix.S32V);
                stream.WriteBool(true);
                stream.WriteU8(0x0A);
                stream.WriteU8(5);
                stream.WriteString("ambient_light", SizePrefix.S16);
                stream.WriteF32(entry.AmbientLight);
                stream.WriteU8(1);
                stream.WriteString("bed_works", SizePrefix.S16);
                stream.WriteBool(true);
                stream.WriteU8(6);
                stream.WriteString("coordinate_scale", SizePrefix.S16);
                stream.WriteF64(1.0);
                stream.WriteU8(8);
                stream.WriteString("effects", SizePrefix.S16);
                stream.WriteString(entry.Effect, SizePrefix.S16);
                stream.WriteU8(1);
                stream.WriteString("has_ceiling", SizePrefix.S16);
                stream.WriteBool(false);
                stream.WriteU8(1);
                stream.WriteString("has_raids", SizePrefix.S16);
                stream.WriteBool(true);
                stream.WriteU8(1);
                stream.WriteString("has_skylight", SizePrefix.S16);
                stream.WriteBool(entry.HasSkylight);
                stream.WriteU8(3);
                stream.WriteString("height", SizePrefix.S16);
                stream.WriteS32(entry.Height);
                stream.WriteU8(8);
                stream.WriteString("infiniburn", SizePrefix.S16);
                stream.WriteString("#minecraft:infiniburn_overworld", SizePrefix.S16);
                stream.WriteU8(3);
                stream.WriteString("logical_height", SizePrefix.S16);
                stream.WriteS32(entry.Height);
                stream.WriteU8(3);
                stream.WriteString("min_y", SizePrefix.S16);
                stream.WriteS32(entry.Depth);
                stream.WriteU8(3);
                stream.WriteString("monster_spawn_block_light_limit", SizePrefix.S16);
                stream.WriteS32(0);
                stream.WriteU8(3);
                stream.WriteString("monster_spawn_light_level", SizePrefix.S16);
                stream.WriteS32(0);
                stream.WriteU8(1);
                stream.WriteString("natural", SizePrefix.S16);
                stream.WriteBool(entry.Natural);
                stream.WriteU8(1);
                stream.WriteString("piglin_safe", SizePrefix.S16);
                stream.WriteBool(entry.PiglinSafe);
                stream.WriteU8(1);
                stream.WriteString("respawn_anchor_works", SizePrefix.S16);
                stream.WriteBool(true);
                stream.WriteU8(1);
                stream.WriteString("ultrawarm", SizePrefix.S16);
                stream.WriteBool(false);
                stream.WriteU8(0);
            }
            Buffer = stream.ToArray();
            DimensionRegistry = [..registry];
        }
        public ClientboundRegistry(RegistryBiome[] registry)
        {
            using MemoryStream stream = new();
            stream.WriteS32V(Id);
            stream.WriteString("minecraft:worldgen/biome", SizePrefix.S32V);
            stream.WriteS32V(registry.Length);
            for (int i = 0; i < registry.Length; i++)
            {
                RegistryBiome entry = registry[i];
                stream.WriteString(entry.Name, SizePrefix.S32V);
                stream.WriteBool(true);
                stream.WriteU8(0x0A);
                stream.WriteU8(1);
                stream.WriteString("has_precipitation", SizePrefix.S16);
                stream.WriteBool(entry.Precipitation);
                stream.WriteU8(5);
                stream.WriteString("temperature", SizePrefix.S16);
                stream.WriteF32(0.0f);
                stream.WriteU8(5);
                stream.WriteString("downfall", SizePrefix.S16);
                stream.WriteF32(0.0f);
                stream.WriteU8(0x0A);
                stream.WriteString("effects", SizePrefix.S16);
                stream.WriteU8(3);
                stream.WriteString("sky_color", SizePrefix.S16);
                stream.WriteU8(entry.SkyColor.A);
                stream.WriteU8(entry.SkyColor.R);
                stream.WriteU8(entry.SkyColor.G);
                stream.WriteU8(entry.SkyColor.B);
                stream.WriteU8(3);
                stream.WriteString("water_fog_color", SizePrefix.S16);
                stream.WriteU8(entry.WaterFogColor.A);
                stream.WriteU8(entry.WaterFogColor.R);
                stream.WriteU8(entry.WaterFogColor.G);
                stream.WriteU8(entry.WaterFogColor.B);
                stream.WriteU8(3);
                stream.WriteString("fog_color", SizePrefix.S16);
                stream.WriteU8(entry.FogColor.A);
                stream.WriteU8(entry.FogColor.R);
                stream.WriteU8(entry.FogColor.G);
                stream.WriteU8(entry.FogColor.B);
                stream.WriteU8(3);
                stream.WriteString("water_color", SizePrefix.S16);
                stream.WriteU8(entry.WaterColor.A);
                stream.WriteU8(entry.WaterColor.R);
                stream.WriteU8(entry.WaterColor.G);
                stream.WriteU8(entry.WaterColor.B);
                if (entry.FoliageColor.HasValue)
                {
                    stream.WriteU8(3);
                    stream.WriteString("foliage_color", SizePrefix.S16);
                    stream.WriteU8(entry.FoliageColor.Value.A);
                    stream.WriteU8(entry.FoliageColor.Value.R);
                    stream.WriteU8(entry.FoliageColor.Value.G);
                    stream.WriteU8(entry.FoliageColor.Value.B);
                }
                if (entry.GrassColor.HasValue)
                {
                    stream.WriteU8(3);
                    stream.WriteString("grass_color", SizePrefix.S16);
                    stream.WriteU8(entry.GrassColor.Value.A);
                    stream.WriteU8(entry.GrassColor.Value.R);
                    stream.WriteU8(entry.GrassColor.Value.G);
                    stream.WriteU8(entry.GrassColor.Value.B);
                }
                if (entry.Music is not null)
                {
                    stream.WriteU8(0x0A);
                    stream.WriteString("music", SizePrefix.S16);
                    stream.WriteU8(1);
                    stream.WriteString("replace_current_music", SizePrefix.S16);
                    stream.WriteBool(entry.Music.ReplaceCurrentMusic);
                    stream.WriteU8(8);
                    stream.WriteString("sound", SizePrefix.S16);
                    stream.WriteString(entry.Music.Sound, SizePrefix.S16);
                    stream.WriteU8(3);
                    stream.WriteString("max_delay", SizePrefix.S16);
                    stream.WriteS32(entry.Music.MaxDelay);
                    stream.WriteU8(3);
                    stream.WriteString("min_delay", SizePrefix.S16);
                    stream.WriteS32(entry.Music.MinDelay);
                    stream.WriteU8(0);
                }
                if (entry.AmbientSound is not null)
                {
                    stream.WriteU8(8);
                    stream.WriteString("ambient_sound", SizePrefix.S16);
                    stream.WriteString(entry.AmbientSound, SizePrefix.S16);
                }
                if (entry.AdditionsSound is not null)
                {
                    stream.WriteU8(0x0A);
                    stream.WriteString("additions_sound", SizePrefix.S16);
                    stream.WriteU8(8);
                    stream.WriteString("sound", SizePrefix.S16);
                    stream.WriteString(entry.AdditionsSound.Sound, SizePrefix.S16);
                    stream.WriteU8(6);
                    stream.WriteString("tick_chance", SizePrefix.S16);
                    stream.WriteF64(entry.AdditionsSound.Chance / 20.0d);
                    stream.WriteU8(0);
                }
                if (entry.MoodSound is not null)
                {
                    stream.WriteU8(0x0A);
                    stream.WriteString("additions_sound", SizePrefix.S16);
                    stream.WriteU8(8);
                    stream.WriteString("sound", SizePrefix.S16);
                    stream.WriteString(entry.MoodSound.Sound, SizePrefix.S16);
                    stream.WriteU8(3);
                    stream.WriteString("tick_delay", SizePrefix.S16);
                    stream.WriteS32(entry.MoodSound.Delay);
                    stream.WriteU8(6);
                    stream.WriteString("offset", SizePrefix.S16);
                    stream.WriteF64(entry.MoodSound.Offset);
                    stream.WriteU8(3);
                    stream.WriteString("block_search_extent", SizePrefix.S16);
                    stream.WriteS32(entry.MoodSound.BlockSearchExtent);
                    stream.WriteU8(0);
                }
                //TODO: particle
                stream.WriteU8(0);
                stream.WriteU8(0);
            }
            Buffer = stream.ToArray();
            BiomeRegistry = [..registry];
        }
        public ClientboundRegistry(RegistryBanner[] registry)
        {
            using MemoryStream stream = new();
            stream.WriteS32V(Id);
            stream.WriteString("minecraft:banner_pattern", SizePrefix.S32V);
            stream.WriteS32V(registry.Length);
            for (int i = 0; i < registry.Length; i++)
            {
                RegistryBanner entry = registry[i];
                stream.WriteString(entry.Name, SizePrefix.S32V);
                stream.WriteBool(true);
                stream.WriteU8(0x0A);
                stream.WriteU8(8);
                stream.WriteString("asset_id", SizePrefix.S16);
                stream.WriteString(entry.Texture, SizePrefix.S16);
                stream.WriteU8(8);
                stream.WriteString("translation_key", SizePrefix.S16);
                stream.WriteString(entry.TranslationKey, SizePrefix.S16);
                stream.WriteU8(0x00);
            }
            Buffer = stream.ToArray();
        }
        public ClientboundRegistry(RegistryChat[] registry)
        {
            using MemoryStream stream = new();
            stream.WriteS32V(Id);
            stream.WriteString("minecraft:chat_type", SizePrefix.S32V);
            stream.WriteS32V(registry.Length);
            for (int i = 0; i < registry.Length; i++)
            {
                RegistryChat entry = registry[i];
                stream.WriteString(entry.Name, SizePrefix.S32V);
                stream.WriteBool(true);
                stream.WriteU8(0x0A);
                stream.WriteU8(0x0A);
                stream.WriteString("chat", SizePrefix.S16);
                stream.WriteU8(0x0A);
                stream.WriteString("style", SizePrefix.S16);
                entry.ChatTranslationStyling.NbtSerialize(stream);
                stream.WriteU8(0x00);
                stream.WriteU8(8);
                stream.WriteString("translation_key", SizePrefix.S16);
                stream.WriteString(entry.ChatTranslation.Key, SizePrefix.S16);
                stream.WriteU8(9);
                stream.WriteString("parameters", SizePrefix.S16);
                stream.WriteU8(8);
                stream.WriteS32(entry.ChatTranslation.Arguments.Length);
                for (int e = 0; e < entry.ChatTranslation.Arguments.Length; e++) stream.WriteString(entry.ChatTranslation.Arguments[e], SizePrefix.S16);
                stream.WriteU8(0x00);
                stream.WriteU8(0x0A);
                stream.WriteString("narration", SizePrefix.S16);
                stream.WriteU8(8);
                stream.WriteString("translation_key", SizePrefix.S16);
                stream.WriteString(entry.NarratorTranslation.Key, SizePrefix.S16);
                stream.WriteU8(9);
                stream.WriteString("parameters", SizePrefix.S16);
                stream.WriteU8(8);
                stream.WriteS32(entry.NarratorTranslation.Arguments.Length);
                for (int e = 0; e < entry.NarratorTranslation.Arguments.Length; e++) stream.WriteString(entry.NarratorTranslation.Arguments[e], SizePrefix.S16);
                stream.WriteU8(0x00);
                stream.WriteU8(0x00);
            }
            ChatRegistry = [.. registry];
            Buffer = stream.ToArray();
        }
        public ClientboundRegistry(RegistryDamage[] registry)
        {
            using MemoryStream stream = new();
            stream.WriteS32V(Id);
            stream.WriteString("minecraft:damage_type", SizePrefix.S32V);
            stream.WriteS32V(registry.Length);
            for (int i = 0; i < registry.Length; i++)
            {
                RegistryDamage entry = registry[i];
                stream.WriteString(entry.Name, SizePrefix.S32V);
                stream.WriteBool(true);
                stream.WriteU8(0x0A);
                stream.WriteU8(8);
                stream.WriteString("effects", SizePrefix.S16);
                stream.WriteString(entry.Effect, SizePrefix.S16);
                stream.WriteU8(5);
                stream.WriteString("exhaustion", SizePrefix.S16);
                stream.WriteF32(0.0f);
                stream.WriteU8(8);
                stream.WriteString("scaling", SizePrefix.S16);
                stream.WriteString("always", SizePrefix.S16);
                stream.WriteU8(8);
                stream.WriteString("message_id", SizePrefix.S16);
                stream.WriteString("arrow", SizePrefix.S16);
                stream.WriteU8(0x00);
            }
            DamageRegistry = [.. registry];
            Buffer = stream.ToArray();
        }        
        public ClientboundRegistry(RegistryWolf[] registry)
        {
            using MemoryStream stream = new();
            stream.WriteS32V(Id);
            stream.WriteString("minecraft:wolf_variant", SizePrefix.S32V);
            stream.WriteS32V(registry.Length);
            for (int i = 0; i < registry.Length; i++)
            {
                RegistryWolf entry = registry[i];
                stream.WriteString(entry.Name, SizePrefix.S32V);
                stream.WriteBool(true);
                stream.WriteU8(0x0A);
                stream.WriteU8(8);
                stream.WriteString("wild_texture", SizePrefix.S16);
                stream.WriteString(entry.TextureWild, SizePrefix.S16);
                stream.WriteU8(8);
                stream.WriteString("tame_texture", SizePrefix.S16);
                stream.WriteString(entry.TextureTame, SizePrefix.S16);
                stream.WriteU8(8);
                stream.WriteString("angry_texture", SizePrefix.S16);
                stream.WriteString(entry.TextureAngry, SizePrefix.S16);
                stream.WriteU8(0x00);
            }
            Buffer = stream.ToArray();
        }
        public ClientboundRegistry(RegistryPainting[] registry)
        {
            using MemoryStream stream = new();
            stream.WriteS32V(Id);
            stream.WriteString("minecraft:painting_variant", SizePrefix.S32V);
            stream.WriteS32V(registry.Length);
            for (int i = 0; i < registry.Length; i++)
            {
                RegistryPainting entry = registry[i];
                stream.WriteString(entry.Name, SizePrefix.S32V);
                stream.WriteBool(true);
                stream.WriteU8(0x0A);
                stream.WriteU8(8);
                stream.WriteString("asset_id", SizePrefix.S16);
                stream.WriteString(entry.Texture, SizePrefix.S16);
                stream.WriteU8(3);
                stream.WriteString("height", SizePrefix.S16);
                stream.WriteS32(entry.Height);
                stream.WriteU8(3);
                stream.WriteString("width", SizePrefix.S16);
                stream.WriteS32(entry.Width);
                stream.WriteU8(0x00);
            }
            Buffer = stream.ToArray();
        }
        public ClientboundRegistry(bool trimOrPattern) //TODO: armor trim pattern and material registries
        {
            using MemoryStream stream = new();
            using MemoryStream packetOut = new();
            packetOut.WriteS32V(PacketIds.OutgoingConfigurationRegistry);
            packetOut.WriteString(trimOrPattern ? "minecraft:trim_material" : "minecraft:trim_pattern", SizePrefix.S32V);
            packetOut.WriteS32V(0);
            Buffer = stream.ToArray();
        }
        internal override void Transform(Client client)
        {
            if (DimensionRegistry is not null) client.DimensionRegistry = DimensionRegistry;
            if (BiomeRegistry is not null) client.BiomeRegistry = BiomeRegistry;
            if (ChatRegistry is not null) client.ChatRegistry = ChatRegistry;
            if (DamageRegistry is not null) client.DamageRegistry = DamageRegistry;
        }
    }
}

