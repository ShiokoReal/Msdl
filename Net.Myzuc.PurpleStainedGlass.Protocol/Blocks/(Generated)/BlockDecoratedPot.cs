namespace Net.Myzuc.PurpleStainedGlass.Protocol.Blocks
{
    public sealed class BlockDecoratedPot : Block
    {
        public enum EnumFacing : int
        {
            North = 0,
            South = 1,
            West = 2,
            East = 3
        }
        public override int BlockId => 26574 + (Waterlogged ? 0 : 1) + (int)Facing * 2 + (Cracked ? 0 : 8);
        public override int LiquidId => Waterlogged ? 1 : 0;
        public override int LightEmission => 0;
        public override int LightFilter => 0;
        public override (double xa, double ya, double za, double xb, double yb, double zb)[] Collisions => [
            (0.0625, 0, 0.0625, 0.9375, 1, 0.9375)
        ];
        public bool Waterlogged = false;
        public EnumFacing Facing = EnumFacing.North;
        public bool Cracked = false;
        public BlockDecoratedPot()
        {
            
        }
        public override BlockDecoratedPot Clone()
        {
            return new()
            {
                Waterlogged = Waterlogged,
                Facing = Facing,
                Cracked = Cracked
            };
        }
        public override Block Break()
        {
            return Waterlogged ? new BlockWater() : new BlockAir();
        }
    }
}
