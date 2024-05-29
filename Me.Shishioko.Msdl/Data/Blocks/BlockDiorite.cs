using Me.Shishioko.Msdl.Data.Types;

namespace Me.Shishioko.Msdl.Data.Blocks
{
    public sealed class BlockDiorite : BlockTypeStone
    {
        public BlockDiorite()
        {

        }
        internal override ushort GetProtocolID() => 4;
        public override string GetName() => "diorite";
    }
}
