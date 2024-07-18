using System.Diagnostics.Contracts;

namespace Me.Shishioko.Msdl.Data
{
    public readonly struct ChunkSectionBlocks
    {
        internal readonly int[] Data;
        internal readonly ushort NotAir;
        public ChunkSectionBlocks(int[] data, ushort notAir)
        {
            Contract.Requires(data.Length == 4096);
            Data = data;
            NotAir = notAir;
        }
        public ChunkSectionBlocks()
        {
            Data = new int[4096];
            NotAir = 0;
        }
    }
}
