namespace Net.Myzuc.PurpleStainedGlass.Protocol.Blocks
{
    public sealed class BlockPottedOxeyeDaisy : Block
    {
        public override int BlockId => 8587;
        public override int LiquidId => 0;
        public override int LightEmission => 0;
        public override int LightFilter => 0;
        public override (double xa, double ya, double za, double xb, double yb, double zb)[] Collisions => [
            (0.3125, 0, 0.3125, 0.6875, 0.375, 0.6875)
        ];
        public BlockPottedOxeyeDaisy()
        {
            
        }
        public override BlockPottedOxeyeDaisy Clone()
        {
            return new();
        }
        public override Block Break()
        {
            return new BlockAir();
        }
    }
}
