namespace Net.Myzuc.PurpleStainedGlass.Protocol.Blocks
{
    public sealed class BlockCrafter : Block
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
        public override int BlockId => 26590 + (Triggered ? 0 : 1) + (int)Orientation * 2 + (Crafting ? 0 : 24);
        public override int LiquidId => 0;
        public override int LightEmission => 0;
        public override int LightFilter => 15;
        public override (double xa, double ya, double za, double xb, double yb, double zb)[] Collisions => [
            (0, 0, 0, 1, 1, 1)
        ];
        public bool Triggered = false;
        public EnumOrientation Orientation = EnumOrientation.DownEast;
        public bool Crafting = false;
        public BlockCrafter()
        {
            
        }
        public override BlockCrafter Clone()
        {
            return new()
            {
                Triggered = Triggered,
                Orientation = Orientation,
                Crafting = Crafting
            };
        }
        public override Block Break()
        {
            return new BlockAir();
        }
    }
}
