using Me.Shishioko.Msdl.Data.Types;

namespace Me.Shishioko.Msdl.Data.Blocks
{
    public sealed class BlockPolishedAndesite : BlockTypeStone
    {
        public BlockPolishedAndesite()
        {

        }
        internal override ushort GetProtocolID() => 7;
        public override string GetName() => "polished_andesite";
    }
}
