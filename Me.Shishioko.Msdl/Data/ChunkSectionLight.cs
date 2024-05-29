using Me.Shishioko.Msdl.Util;
using System.Diagnostics.Contracts;

namespace Me.Shishioko.Msdl.Data
{
    public readonly struct ChunkSectionLight
    {
        internal readonly CompactArray? Data;
        public ChunkSectionLight(CompactArray? data)
        {
            if (data == null) Data = new(4, 4096);
            else
            {
                Contract.Requires(data.Bits == 4);
                Contract.Requires(data.Length == 4096);
                Data = data;
            }
        }
    }
}
