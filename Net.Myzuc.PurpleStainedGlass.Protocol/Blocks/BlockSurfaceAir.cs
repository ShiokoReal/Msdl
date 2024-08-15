namespace Net.Myzuc.PurpleStainedGlass.Protocol.Blocks
{
    public sealed class BlockSurfaceAir : BlockAir
    {
        public override int Id => 0;
        public BlockSurfaceAir()
        {

        }
        public override BlockSurfaceAir Clone() => new();
    }
}
