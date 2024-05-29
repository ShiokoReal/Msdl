using Me.Shishioko.Msdl.Data.Types;

namespace Me.Shishioko.Msdl.Data.Blocks
{
    public sealed class BlockVoidAir : BlockTypeAir
    {
        public BlockVoidAir()
        {

        }
        internal override ushort GetProtocolID() => 729;
        public override string GetName() => "void_air";
    }
}
