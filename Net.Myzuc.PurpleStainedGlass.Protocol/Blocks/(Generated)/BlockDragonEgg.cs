namespace Net.Myzuc.PurpleStainedGlass.Protocol.Blocks
{
    public sealed class BlockDragonEgg : Block
    {
        public override int BlockId => 7416;
        public override int LiquidId => 0;
        public override int LightEmission => 1;
        public override int LightFilter => 0;
        public override (double xa, double ya, double za, double xb, double yb, double zb)[] Collisions => [
            (0.0625, 0, 0.0625, 0.9375, 1, 0.9375)
        ];
        public BlockDragonEgg()
        {
            
        }
        public override BlockDragonEgg Clone()
        {
            return new();
        }
        public override Block Break()
        {
            return new BlockAir();
        }
    }
}
