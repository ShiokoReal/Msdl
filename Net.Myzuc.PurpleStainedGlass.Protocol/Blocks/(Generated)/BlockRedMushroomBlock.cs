namespace Net.Myzuc.PurpleStainedGlass.Protocol.Blocks
{
    public sealed class BlockRedMushroomBlock : Block
    {
        public override int BlockId => 6613 + (West ? 0 : 1) + (Up ? 0 : 2) + (South ? 0 : 4) + (North ? 0 : 8) + (East ? 0 : 16) + (Down ? 0 : 32);
        public override int LiquidId => 0;
        public override int LightEmission => 0;
        public override int LightFilter => 15;
        public override (double xa, double ya, double za, double xb, double yb, double zb)[] Collisions => [
            (0, 0, 0, 1, 1, 1)
        ];
        public bool West = false;
        public bool Up = false;
        public bool South = false;
        public bool North = false;
        public bool East = false;
        public bool Down = false;
        public BlockRedMushroomBlock()
        {
            
        }
        public override BlockRedMushroomBlock Clone()
        {
            return new()
            {
                West = West,
                Up = Up,
                South = South,
                North = North,
                East = East,
                Down = Down
            };
        }
        public override Block Break()
        {
            return new BlockAir();
        }
    }
}
