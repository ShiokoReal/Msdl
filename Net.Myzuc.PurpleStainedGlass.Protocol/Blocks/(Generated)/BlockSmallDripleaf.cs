namespace Net.Myzuc.PurpleStainedGlass.Protocol.Blocks
{
    public sealed class BlockSmallDripleaf : Block
    {
        public enum EnumHalf : int
        {
            Upper = 0,
            Lower = 1
        }
        public enum EnumFacing : int
        {
            North = 0,
            South = 1,
            West = 2,
            East = 3
        }
        public override int BlockId => 24884 + (Waterlogged ? 0 : 1) + (int)Half * 2 + (int)Facing * 4;
        public override int LiquidId => Waterlogged ? 1 : 0;
        public override int LightEmission => 0;
        public override int LightFilter => 0;
        public override (double xa, double ya, double za, double xb, double yb, double zb)[] Collisions => [];
        public bool Waterlogged = false;
        public EnumHalf Half = EnumHalf.Upper;
        public EnumFacing Facing = EnumFacing.North;
        public BlockSmallDripleaf()
        {
            
        }
        public override BlockSmallDripleaf Clone()
        {
            return new()
            {
                Waterlogged = Waterlogged,
                Half = Half,
                Facing = Facing
            };
        }
        public override Block Break()
        {
            return Waterlogged ? new BlockWater() : new BlockAir();
        }
    }
}
