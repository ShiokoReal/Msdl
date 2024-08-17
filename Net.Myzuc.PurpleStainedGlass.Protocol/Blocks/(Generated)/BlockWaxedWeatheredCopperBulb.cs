namespace Net.Myzuc.PurpleStainedGlass.Protocol.Blocks
{
    public sealed class BlockWaxedWeatheredCopperBulb : Block
    {
        public override int BlockId => 24716 + (Powered ? 0 : 1) + (Lit ? 0 : 2);
        public override int LiquidId => 0;
        public override int LightEmission => 0;
        public override int LightFilter => 15;
        public override (double xa, double ya, double za, double xb, double yb, double zb)[] Collisions => [
            (0, 0, 0, 1, 1, 1)
        ];
        public bool Powered = false;
        public bool Lit = false;
        public BlockWaxedWeatheredCopperBulb()
        {
            
        }
        public override BlockWaxedWeatheredCopperBulb Clone()
        {
            return new()
            {
                Powered = Powered,
                Lit = Lit
            };
        }
        public override Block Break()
        {
            return new BlockAir();
        }
    }
}
