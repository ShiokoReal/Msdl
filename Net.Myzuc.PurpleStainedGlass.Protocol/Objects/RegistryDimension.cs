using System;

namespace Net.Myzuc.PurpleStainedGlass.Protocol.Objects
{
    public sealed class RegistryDimension
    {
        public static readonly string[] Effects = ["minecraft:overworld", "minecraft:the_nether", "minecraft:the_end"];
        public string Name { get; }
        public int Height { get; }
        public int Depth { get; }
        public bool HasSkylight { get; init; } = true;
        public float AmbientLight { get; init; } = 0.0f;
        public string Effect { get; init; } = "minecraft:overworld";
        public bool Natural { get; init; } = true;
        public bool PiglinSafe { get; init; } = true;
        public RegistryDimension(string name, int height, int depth)
        {
            if (height <= 0 || height % 16 != 0) throw new ArgumentException();
            if (height + depth >= 2032 || depth < -2032) throw new ArgumentException();
            Name = name;
            Height = height;
            Depth = depth;
        }
    }
}
