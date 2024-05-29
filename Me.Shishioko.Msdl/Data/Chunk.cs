using Me.Shishioko.Msdl.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Me.Shishioko.Msdl.Content
{
    public readonly struct Chunk
    {
        public sealed class BlockSection
        {
            internal readonly int[] Data = new int[4096];
            internal ushort NonZero = 4096;
            internal BlockSection()
            {

            }
            public int this[int x, int y, int z]
            {
                get
                {
                    Contract.Requires(x >= 0);
                    Contract.Requires(y >= 0);
                    Contract.Requires(z >= 0);
                    Contract.Requires(x < 16);
                    Contract.Requires(y < 16);
                    Contract.Requires(z < 16);
                    return Data[y << 8 | z << 4 | x];
                }
                set
                {
                    Contract.Requires(x >= 0);
                    Contract.Requires(y >= 0);
                    Contract.Requires(z >= 0);
                    Contract.Requires(x < 16);
                    Contract.Requires(y < 16);
                    Contract.Requires(z < 16);
                    int index = y << 8 | z << 4 | x;
                    if (Data[index] == value) return;
                    if (Data[index] == 0) NonZero++; //TODO: void air and cave air
                    if (value == 0) NonZero--;
                    Data[index] = value;
                }
            }
            internal void Serialize(DataStream dataOut)
            {
                
            }
        }
        public readonly int X;
        public readonly int Y;
        public readonly int Z;
        public readonly int[] MotionBlocking;
        public readonly BlockSection[] Blocks;
        public readonly BiomeSection[] Biomes;
        public readonly LightSection[] Skylight;
        public readonly LightSection[] Blocklight;
        public Chunk(int x, int y, int z, int height)
        {
            Contract.Requires(x < 2097152);
            Contract.Requires(z < 2097152);
            Contract.Requires(x >= -2097152);
            Contract.Requires(z >= -2097152);
            Contract.Requires(height >= 0);
            Contract.Requires(height + y < 128);
            Contract.Requires(y >= -128);
            X = x;
            Y = y;
            Z = z;
            MotionBlocking = new int[256];
            Blocks = new BlockSection[height];
            Biomes = new BiomeSection[height];
            Skylight = new LightSection[height + 2];
            Blocklight = new LightSection[height + 2];
        }
    }
}
