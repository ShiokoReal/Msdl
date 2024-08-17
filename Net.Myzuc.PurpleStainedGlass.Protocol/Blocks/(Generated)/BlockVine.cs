namespace Net.Myzuc.PurpleStainedGlass.Protocol.Blocks
{
    public sealed class BlockVine : Block
    {
        public override int BlockId => 6837 + (West ? 0 : 1) + (Up ? 0 : 2) + (South ? 0 : 4) + (North ? 0 : 8) + (East ? 0 : 16);
        public override int LiquidId => 0;
        public override int LightEmission => 0;
        public override int LightFilter => 0;
        public override (double xa, double ya, double za, double xb, double yb, double zb)[] Collisions => [];
        public bool West = false;
        public bool Up = false;
        public bool South = false;
        public bool North = false;
        public bool East = false;
        public BlockVine()
        {
            
        }
        public override BlockVine Clone()
        {
            return new()
            {
                West = West,
                Up = Up,
                South = South,
                North = North,
                East = East
            };
        }
        public override Block Break()
        {
            return new BlockAir();
        }
    }
}
