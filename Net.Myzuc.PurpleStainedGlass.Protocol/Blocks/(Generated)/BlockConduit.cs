namespace Net.Myzuc.PurpleStainedGlass.Protocol.Blocks
{
    public sealed class BlockConduit : Block
    {
        public override int BlockId => 12942 + (Waterlogged ? 0 : 1);
        public override int LiquidId => Waterlogged ? 1 : 0;
        public override int LightEmission => 15;
        public override int LightFilter => 1;
        public override (double xa, double ya, double za, double xb, double yb, double zb)[] Collisions => [
            (0.3125, 0.3125, 0.3125, 0.6875, 0.6875, 0.6875)
        ];
        public bool Waterlogged = false;
        public BlockConduit()
        {
            
        }
        public override BlockConduit Clone()
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
