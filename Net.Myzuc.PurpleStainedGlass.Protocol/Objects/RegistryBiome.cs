using System;
using System.Drawing;

namespace Net.Myzuc.PurpleStainedGlass.Protocol.Objects
{
    public sealed class RegistryBiome
    {
        public sealed class BiomeMusic //TODO: does this actually work?
        {
            public string Sound { get; }
            public bool ReplaceCurrentMusic { get; init; } = true;
            public int MaxDelay { get; init; } = 0;
            public int MinDelay { get; init; } = 0;
            public BiomeMusic(string sound)
            {
                Sound = sound;
            }
        }
        public sealed class BiomeRandomSound //TODO: does this actually work?
        {
            public string Sound { get; }
            public double Chance { get; init; } = 1.0d / 60.0d;
            public BiomeRandomSound(string sound)
            {
                Sound = sound;
            }
        }
        public sealed class BiomeIntervalSound //TODO: does this actually work?
        {
            public string Sound { get; }
            public int Delay { get; init; } = 0;
            public double Offset { get; init; } = 0.0d;
            public int BlockSearchExtent { get; init; } = 0;
            public BiomeIntervalSound(string sound)
            {
                Sound = sound;
            }
        }
        public string Name { get; }
        public bool Precipitation { get; init; } = true;
        public Color SkyColor { get; init; } = Color.CornflowerBlue;
        public Color WaterFogColor { get; init; } = Color.DarkBlue;
        public Color FogColor { get; init; } = Color.WhiteSmoke;
        public Color WaterColor { get; init; } = Color.Blue;
        public Color? FoliageColor { get; init; } = Color.LimeGreen;
        public Color? GrassColor { get; init; } = Color.LimeGreen;
        public BiomeMusic? Music { get; init; } = null;
        public string? AmbientSound { get; init; } = null;
        public BiomeRandomSound? AdditionsSound { get; init; } = null;
        public BiomeIntervalSound? MoodSound { get; init; } = null;
        public RegistryBiome(string name)
        {
            Name = name;
        }
        //TODO: particle (float:probability,compound:properties+type:particle_type)
    }
}
