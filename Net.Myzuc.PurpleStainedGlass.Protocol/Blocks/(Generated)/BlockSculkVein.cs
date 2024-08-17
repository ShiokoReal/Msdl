namespace Net.Myzuc.PurpleStainedGlass.Protocol.Blocks
{
    public sealed class BlockSculkVein : Block
    {
        public override int BlockId => 22800 + (West ? 0 : 1) + (Waterlogged ? 0 : 2) + (Up ? 0 : 4) + (South ? 0 : 8) + (North ? 0 : 16) + (East ? 0 : 32) + (Down ? 0 : 64);
        public override int LiquidId => Waterlogged ? 1 : 0;
        public override int LightEmission => 0;
        public override int LightFilter => 1;
        public override (double xa, double ya, double za, double xb, double yb, double zb)[] Collisions => [];
        public bool West = false;
        public bool Waterlogged = false;
        public bool Up = false;
        public bool South = false;
        public bool North = false;
        public bool East = false;
        public bool Down = false;
        public BlockSculkVein()
        {
            
        }
        public override BlockSculkVein Clone()
        {
            return new()
            {
                West = West,
                Waterlogged = Waterlogged,
                Up = Up,
                South = South,
                North = North,
                East = East,
                Down = Down
            };
        }
        public override Block Break()
        {
            return Waterlogged ? new BlockWater() : new BlockAir();
        }
    }
}
