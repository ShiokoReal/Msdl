using Me.Shishioko.Msdl.Data.Types;

namespace Me.Shishioko.Msdl.Data.Blocks
{
    public sealed class BlockAir : BlockTypeAir
    {
        public BlockAir()
        {

        }
        internal override ushort GetProtocolID() => 0;
        public override string GetName() => "air";
    }
}
