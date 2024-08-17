namespace Net.Myzuc.PurpleStainedGlass.Protocol.Blocks
{
    public sealed class BlockEndGateway : Block
    {
        public override int BlockId => 12514;
        public override int LiquidId => 0;
        public override int LightEmission => 15;
        public override int LightFilter => 1;
        public override (double xa, double ya, double za, double xb, double yb, double zb)[] Collisions => [];
        public BlockEndGateway()
        {
            
        }
        public override BlockEndGateway Clone()
        {
            return new();
        }
        public override Block Break()
        {
            return new BlockAir();
        }
    }
}
