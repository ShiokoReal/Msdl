using Me.Shishioko.Msdl.Data;
using Net.Myzuc.ShioLib;
using System.Diagnostics.Contracts;

namespace Me.Shishioko.Msdl.Test
{
    internal class Chunk
    {
        public readonly int X;
        public readonly int Z;
        public readonly World World;
        internal readonly int[][] Blocks;
        internal readonly int[][] Biomes;
        internal readonly SemiCompactArray?[] Skylight;
        internal readonly SemiCompactArray?[] Blocklight;
        internal readonly SemiCompactArray MotionBlocking;
        public Chunk(int x, int z, World world)
        {
            X = x;
            Z = z;
            World = world;
            int sections = world.Object.Height / 16;
            Blocks = new int[sections][];
            Biomes = new int[sections][];
            Skylight = new SemiCompactArray?[sections + 2];
            Blocklight = new SemiCompactArray?[sections + 2];
            for (int i = 0; i < sections; i++)
            {
                Blocks[i] = new int[4096];
                Biomes[i] = new int[64];
                Blocklight[i + 1] = new(4, 4096);
                for (int e = 0; e < 4096; e++)
                {
                    Blocklight[i + 1]![e] = 15;
                }
                Skylight[i + 1] = new(4, 4096);
            }
            MotionBlocking = new SemiCompactArray((byte)Math.Ceiling(Math.Log2(World.Object.Height)), 256);
        }
        public int this[int x, int y, int z]
        {
            get
            {
                int yy = y - World.Object.Depth;
                Contract.Assert(x >= 0 && x < 16);
                Contract.Assert(z >= 0 && z < 16);
                Contract.Assert(yy >= 0 && yy < World.Object.Height);
                int i = y / 16;
                int yyy = yy % 16;
                return Blocks[i][x | (z << 4) | (yyy << 8)];
            }
            set
            {
                int yy = y - World.Object.Depth;
                Contract.Assert(x >= 0 && x < 16);
                Contract.Assert(z >= 0 && z < 16);
                Contract.Assert(yy >= 0 && yy < World.Object.Height);
                int i = y / 16;
                int yyy = yy % 16;
                Blocks[i][x | (z << 4) | (yyy << 8)] = value;
            }
        }
        public void SetBiome(int x, int y, int z, Biome value)
        {
            int yy = y - World.Object.Depth;
            Contract.Assert(x >= 0 && x < 16);
            Contract.Assert(z >= 0 && z < 16);
            Contract.Assert(yy >= 0 && yy < World.Object.Height);
            int i = y / 16;
            int yyy = yy % 16;
            int id = Array.IndexOf([..World.Game.Biomes], value);
            if (id < 0) throw new ArgumentException();
            Biomes[i][(x/4) | ((z/4) << 2) | ((yyy/4) << 4)] = id;
        }
    }
}
