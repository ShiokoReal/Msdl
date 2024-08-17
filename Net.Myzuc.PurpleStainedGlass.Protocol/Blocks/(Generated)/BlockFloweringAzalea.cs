namespace Net.Myzuc.PurpleStainedGlass.Protocol.Blocks
{
    public sealed class BlockFloweringAzalea : Block
    {
        public override int BlockId => 24825;
        public override int LiquidId => 0;
        public override int LightEmission => 0;
        public override int LightFilter => 0;
        public override (double xa, double ya, double za, double xb, double yb, double zb)[] Collisions => [
            (0.375, 0, 0.375, 0.625, 1, 0.625),
            (0, 0.5, 0, 0.375, 1, 1),
            (0.375, 0.5, 0, 1, 1, 0.375),
            (0.375, 0.5, 0.625, 1, 1, 1),
            (0.625, 0.5, 0.375, 1, 1, 0.625)
        ];
        public BlockFloweringAzalea()
        {
            
        }
        public override BlockFloweringAzalea Clone()
        {
            return new();
        }
        public override Block Break()
        {
            return new BlockAir();
        }
    }
}
