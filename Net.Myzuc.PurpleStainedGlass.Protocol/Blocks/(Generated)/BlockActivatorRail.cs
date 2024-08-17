namespace Net.Myzuc.PurpleStainedGlass.Protocol.Blocks
{
    public sealed class BlockActivatorRail : Block
    {
        public enum EnumShape : int
        {
            NorthSouth = 0,
            EastWest = 1,
            AscendingEast = 2,
            AscendingWest = 3,
            AscendingNorth = 4,
            AscendingSouth = 5
        }
        public override int BlockId => 9320 + (Waterlogged ? 0 : 1) + (int)Shape * 2 + (Powered ? 0 : 12);
        public override int LiquidId => Waterlogged ? 1 : 0;
        public override int LightEmission => 0;
        public override int LightFilter => 0;
        public override (double xa, double ya, double za, double xb, double yb, double zb)[] Collisions => [];
        public bool Waterlogged = false;
        public EnumShape Shape = EnumShape.NorthSouth;
        public bool Powered = false;
        public BlockActivatorRail()
        {
            
        }
        public override BlockActivatorRail Clone()
        {
            return new()
            {
                Waterlogged = Waterlogged,
                Shape = Shape,
                Powered = Powered
            };
        }
        public override Block Break()
        {
            return Waterlogged ? new BlockWater() : new BlockAir();
        }
    }
}
