namespace Net.Myzuc.PurpleStainedGlass.Protocol.Blocks
{
    public sealed class BlockEndPortal : Block
    {
        public override int BlockId => 7406;
        public override int LiquidId => 0;
        public override int LightEmission => 15;
        public override int LightFilter => 0;
        public override (double xa, double ya, double za, double xb, double yb, double zb)[] Collisions => [];
        public BlockEndPortal()
        {
            
        }
        public override BlockEndPortal Clone()
        {
            return new();
        }
        public override Block Break()
        {
            return new BlockAir();
        }
    }
}
