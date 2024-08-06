using Me.Shishioko.Msdl.Data;
using Net.Myzuc.ShioLib;
using System.Data;
using System.Diagnostics.Contracts;
using System.IO.Compression;

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
                Skylight[i + 1] = new(4, 4096);
            }
            MotionBlocking = new SemiCompactArray((byte)Math.Ceiling(Math.Log2(World.Object.Height)), 256);
        }
        internal async Task SaveAsync()
        {
            string path = Path.Combine(Environment.CurrentDirectory, "save", World.Name.Replace(':', '.'));
            Directory.CreateDirectory(path).Create();
            using MemoryStream dataOut = new();
            dataOut.WriteS32V(0);
            for (int i = -1; i < World.Object.Height / 16 + 1; i++)
            {
                if (i >= 0 && i < World.Object.Height / 16)
                {
                    dataOut.WriteS32A(Blocks[i], 4096);
                    dataOut.WriteS32A(Biomes[i], 4096);
                }
                SemiCompactArray? skylight = Skylight[i + 1];
                dataOut.WriteBool(skylight is not null);
                if (skylight is not null)
                {
                    dataOut.WriteU64A(skylight.Data);
                }
                SemiCompactArray? blocklight = Blocklight[i + 1];
                dataOut.WriteBool(blocklight is not null);
                if (blocklight is not null)
                {
                    dataOut.WriteU64A(blocklight.Data);
                }
            }
            using Stream fileOut = new GZipStream(new FileStream(Path.Combine(path, $"{X}x{Z}.bin"), FileMode.OpenOrCreate, FileAccess.Write, FileShare.None), CompressionLevel.Optimal, false);
            await fileOut.WriteU8AAsync(dataOut.ToArray());
        }
        internal async Task<bool> LoadAsync()
        {
            string path = Path.Combine(Environment.CurrentDirectory, "save", World.Name.Replace(':', '.'), $"{X}x{Z}.bin");
            if (!File.Exists(path)) return false;
            using Stream dataIn = new GZipStream(new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.None), CompressionMode.Decompress, false);
            int version = dataIn.ReadS32V();
            for (int i = -1; i < World.Object.Height / 16 + 1; i++)
            {
                if (i >= 0 && i < World.Object.Height / 16)
                {
                    Blocks[i] = dataIn.ReadS32A(4096);
                    Biomes[i] = dataIn.ReadS32A(64);
                }
                SemiCompactArray? skylight = null;
                if (dataIn.ReadBool()) skylight = new(4, 4096, dataIn.ReadU64A(256));
                Skylight[i + 1] = skylight;
                SemiCompactArray? blocklight = null;
                if (dataIn.ReadBool()) blocklight = new(4, 4096, dataIn.ReadU64A(256));
                Blocklight[i + 1] = blocklight;
            }
            return true;
        }
        internal void Generate()
        {
            int sections = World.Object.Height / 16;
            for (int i = 0; i < sections; i++)
            {
                for (int e = 0; e < 4096; e++)
                {
                    Blocklight[i + 1]![e] = 15;
                }
            }
            for (int y = World.Object.Depth; y < World.Object.Depth + World.Object.Height; y++)
            {
                int block = 0;
                if (y < 64) block = 1;
                else if (y == 64) block = 2;
                else if (y <= 67) block = 95;
                for (int xx = 0; xx < 16; xx++)
                {
                    for (int zz = 0; zz < 16; zz++)
                    {
                        this[xx, y, zz] = block;
                    }
                }
            }
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
