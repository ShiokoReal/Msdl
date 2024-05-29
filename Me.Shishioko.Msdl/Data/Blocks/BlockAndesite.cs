using Me.Shishioko.Msdl.Data.Types;

namespace Me.Shishioko.Msdl.Data.Blocks
{
    public sealed class BlockAndesite : BlockTypeStone
    {
        public BlockAndesite()
        {

        }
        internal override ushort GetProtocolID() => 6;
        public override string GetName() => "andesite";
    }
}
