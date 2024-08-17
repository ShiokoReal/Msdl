namespace Net.Myzuc.PurpleStainedGlass.Protocol.Blocks
{
    public sealed class BlockBlackCandleCake : Block
    {
        public override int BlockId => 21029 + (Lit ? 0 : 1);
        public override int LiquidId => 0;
        public override int LightEmission => 0;
        public override int LightFilter => 0;
        public override (double xa, double ya, double za, double xb, double yb, double zb)[] Collisions => [
            (0.0625, 0, 0.0625, 0.9375, 0.5, 0.9375),
            (0.4375, 0.5, 0.4375, 0.5625, 0.875, 0.5625)
        ];
        public bool Lit = false;
        public BlockBlackCandleCake()
        {
            
        }
        public override BlockBlackCandleCake Clone()
        {
            return new()
            {
                Lit = Lit
            };
        }
        public override Block Break()
        {
            return new BlockAir();
        }
    }
}
