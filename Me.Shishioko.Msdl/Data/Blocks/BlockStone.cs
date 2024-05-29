using Me.Shishioko.Msdl.Data.Types;

namespace Me.Shishioko.Msdl.Data.Blocks
{
    public sealed class BlockStone : BlockTypeStone
    {
        public BlockStone()
        {

        }
        internal override ushort GetProtocolID() => 1;
        public override string GetName() => "stone";
    }
}
