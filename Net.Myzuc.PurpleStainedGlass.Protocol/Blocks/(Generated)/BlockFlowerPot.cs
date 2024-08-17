namespace Net.Myzuc.PurpleStainedGlass.Protocol.Blocks
{
    public sealed class BlockFlowerPot : Block
    {
        public override int BlockId => 8567;
        public override int LiquidId => 0;
        public override int LightEmission => 0;
        public override int LightFilter => 0;
        public override (double xa, double ya, double za, double xb, double yb, double zb)[] Collisions => [
            (0.3125, 0, 0.3125, 0.6875, 0.375, 0.6875)
        ];
        public BlockFlowerPot()
        {
            
        }
        public override BlockFlowerPot Clone()
        {
            return new();
        }
        public override Block Break()
        {
            return new BlockAir();
        }
    }
}
