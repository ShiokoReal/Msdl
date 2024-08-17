namespace Net.Myzuc.PurpleStainedGlass.Protocol.Blocks
{
    public sealed class BlockJigsaw : Block
    {
        public enum EnumOrientation : int
        {
            DownEast = 0,
            DownNorth = 1,
            DownSouth = 2,
            DownWest = 3,
            UpEast = 4,
            UpNorth = 5,
            UpSouth = 6,
            UpWest = 7,
            WestUp = 8,
            EastUp = 9,
            NorthUp = 10,
            SouthUp = 11
        }
        public override int BlockId => 19360 + (int)Orientation * 1;
        public override int LiquidId => 0;
        public override int LightEmission => 0;
        public override int LightFilter => 15;
        public override (double xa, double ya, double za, double xb, double yb, double zb)[] Collisions => [
            (0, 0, 0, 1, 1, 1)
        ];
        public EnumOrientation Orientation = EnumOrientation.DownEast;
        public BlockJigsaw()
        {
            
        }
        public override BlockJigsaw Clone()
        {
            return new()
            {
                Orientation = Orientation
            };
        }
        public override Block Break()
        {
            return new BlockAir();
        }
    }
}
