using Me.Shishioko.Msdl.Data.Types;

namespace Me.Shishioko.Msdl.Data.Blocks
{
    public sealed class BlockCaveAir : BlockTypeAir
    {
        public BlockCaveAir()
        {

        }
        internal override ushort GetProtocolID() => 730;
        public override string GetName() => "cave_air";
    }
}
