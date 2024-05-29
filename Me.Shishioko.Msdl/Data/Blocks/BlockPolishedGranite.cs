using Me.Shishioko.Msdl.Data.Types;

namespace Me.Shishioko.Msdl.Data.Blocks
{
    public sealed class BlockPolishedGranite : BlockTypeStone
    {
        public BlockPolishedGranite()
        {

        }
        internal override ushort GetProtocolID() => 3;
        public override string GetName() => "polished_granite";
    }
}
