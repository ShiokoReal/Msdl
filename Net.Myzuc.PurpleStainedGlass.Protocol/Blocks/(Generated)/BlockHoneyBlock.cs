namespace Net.Myzuc.PurpleStainedGlass.Protocol.Blocks
{
    public sealed class BlockHoneyBlock : Block
    {
        public override int BlockId => 19445;
        public override int LiquidId => 0;
        public override int LightEmission => 0;
        public override int LightFilter => 1;
        public override (double xa, double ya, double za, double xb, double yb, double zb)[] Collisions => [
            (0.0625, 0, 0.0625, 0.9375, 0.9375, 0.9375)
        ];
        public BlockHoneyBlock()
        {
            
        }
        public override BlockHoneyBlock Clone()
        {
            return new();
        }
        public override Block Break()
        {
            return new BlockAir();
        }
    }
}
