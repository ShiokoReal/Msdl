namespace Net.Myzuc.PurpleStainedGlass.Protocol.Blocks
{
    public sealed class BlockTorch : Block
    {
        public override int BlockId => 2355;
        public override int LiquidId => 0;
        public override int LightEmission => 14;
        public override int LightFilter => 0;
        public override (double xa, double ya, double za, double xb, double yb, double zb)[] Collisions => [];
        public BlockTorch()
        {
            
        }
        public override BlockTorch Clone()
        {
            return new();
        }
        public override Block Break()
        {
            return new BlockAir();
        }
    }
}
