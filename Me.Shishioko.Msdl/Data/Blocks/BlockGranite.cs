using Me.Shishioko.Msdl.Data.Types;

namespace Me.Shishioko.Msdl.Data.Blocks
{
    public sealed class BlockGranite : BlockTypeStone
    {
        public BlockGranite()
        {

        }
        internal override ushort GetProtocolID() => 2;
        public override string GetName() => "granite";
    }
}
