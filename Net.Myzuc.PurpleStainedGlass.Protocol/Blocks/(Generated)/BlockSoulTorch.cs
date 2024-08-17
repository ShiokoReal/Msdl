namespace Net.Myzuc.PurpleStainedGlass.Protocol.Blocks
{
    public sealed class BlockSoulTorch : Block
    {
        public override int BlockId => 5858;
        public override int LiquidId => 0;
        public override int LightEmission => 10;
        public override int LightFilter => 0;
        public override (double xa, double ya, double za, double xb, double yb, double zb)[] Collisions => [];
        public BlockSoulTorch()
        {
            
        }
        public override BlockSoulTorch Clone()
        {
            return new();
        }
        public override Block Break()
        {
            return new BlockAir();
        }
    }
}
