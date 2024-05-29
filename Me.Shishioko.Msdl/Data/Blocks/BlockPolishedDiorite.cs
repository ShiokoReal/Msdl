using Me.Shishioko.Msdl.Data.Types;

namespace Me.Shishioko.Msdl.Data.Blocks
{
    public sealed class BlockPolishedDiorite : BlockTypeStone
    {
        public BlockPolishedDiorite()
        {

        }
        internal override ushort GetProtocolID() => 5;
        public override string GetName() => "polished_diorite";
    }
}
