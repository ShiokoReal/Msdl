namespace Net.Myzuc.PurpleStainedGlass.Protocol.Blocks
{
    public sealed class BlockCobweb : Block
    {
        public override int BlockId => 2004;
        public override int LiquidId => 0;
        public override int LightEmission => 0;
        public override int LightFilter => 1;
        public override (double xa, double ya, double za, double xb, double yb, double zb)[] Collisions => [];
        public BlockCobweb()
        {
            
        }
        public override BlockCobweb Clone()
        {
            return new();
        }
        public override Block Break()
        {
            return new BlockAir();
        }
    }
}
