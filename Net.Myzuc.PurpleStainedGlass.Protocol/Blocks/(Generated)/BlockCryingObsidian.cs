namespace Net.Myzuc.PurpleStainedGlass.Protocol.Blocks
{
    public sealed class BlockCryingObsidian : Block
    {
        public override int BlockId => 19449;
        public override int LiquidId => 0;
        public override int LightEmission => 10;
        public override int LightFilter => 15;
        public override (double xa, double ya, double za, double xb, double yb, double zb)[] Collisions => [
            (0, 0, 0, 1, 1, 1)
        ];
        public BlockCryingObsidian()
        {
            
        }
        public override BlockCryingObsidian Clone()
        {
            return new();
        }
        public override Block Break()
        {
            return new BlockAir();
        }
    }
}
