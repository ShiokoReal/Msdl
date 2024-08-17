namespace Net.Myzuc.PurpleStainedGlass.Protocol.Blocks
{
    public sealed class BlockSlimeBlock : Block
    {
        public override int BlockId => 10364;
        public override int LiquidId => 0;
        public override int LightEmission => 0;
        public override int LightFilter => 1;
        public override (double xa, double ya, double za, double xb, double yb, double zb)[] Collisions => [
            (0, 0, 0, 1, 1, 1)
        ];
        public BlockSlimeBlock()
        {
            
        }
        public override BlockSlimeBlock Clone()
        {
            return new();
        }
        public override Block Break()
        {
            return new BlockAir();
        }
    }
}
