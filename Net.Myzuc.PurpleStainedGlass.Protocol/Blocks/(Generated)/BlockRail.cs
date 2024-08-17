namespace Net.Myzuc.PurpleStainedGlass.Protocol.Blocks
{
    public sealed class BlockRail : Block
    {
        public enum EnumShape : int
        {
            NorthSouth = 0,
            EastWest = 1,
            AscendingEast = 2,
            AscendingWest = 3,
            AscendingNorth = 4,
            AscendingSouth = 5,
            SouthEast = 6,
            SouthWest = 7,
            NorthWest = 8,
            NorthEast = 9
        }
        public override int BlockId => 4662 + (Waterlogged ? 0 : 1) + (int)Shape * 2;
        public override int LiquidId => Waterlogged ? 1 : 0;
        public override int LightEmission => 0;
        public override int LightFilter => 0;
        public override (double xa, double ya, double za, double xb, double yb, double zb)[] Collisions => [];
        public bool Waterlogged = false;
        public EnumShape Shape = EnumShape.NorthSouth;
        public BlockRail()
        {
            
        }
        public override BlockRail Clone()
        {
            return new()
            {
                Waterlogged = Waterlogged,
                Shape = Shape
            };
        }
        public override Block Break()
        {
            return Waterlogged ? new BlockWater() : new BlockAir();
        }
    }
}
