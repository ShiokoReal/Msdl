namespace Net.Myzuc.PurpleStainedGlass.Protocol.Blocks
{
    public sealed class BlockWeatheredCopperGrate : Block
    {
        public override int BlockId => 24680 + (Waterlogged ? 0 : 1);
        public override int LiquidId => Waterlogged ? 1 : 0;
        public override int LightEmission => 0;
        public override int LightFilter => 0;
        public override (double xa, double ya, double za, double xb, double yb, double zb)[] Collisions => [
            (0, 0, 0, 1, 1, 1)
        ];
        public bool Waterlogged = false;
        public BlockWeatheredCopperGrate()
        {
            
        }
        public override BlockWeatheredCopperGrate Clone()
        {
            return new()
            {
                Waterlogged = Waterlogged
            };
        }
        public override Block Break()
        {
            return Waterlogged ? new BlockWater() : new BlockAir();
        }
    }
}