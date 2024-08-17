namespace Net.Myzuc.PurpleStainedGlass.Protocol.Blocks
{
    public sealed class BlockMovingPiston : Block
    {
        public enum EnumType : int
        {
            Normal = 0,
            Sticky = 1
        }
        public enum EnumFacing : int
        {
            North = 0,
            East = 1,
            South = 2,
            West = 3,
            Up = 4,
            Down = 5
        }
        public override int BlockId => 2063 + (int)Type * 1 + (int)Facing * 2;
        public override int LiquidId => 0;
        public override int LightEmission => 0;
        public override int LightFilter => 0;
        public override (double xa, double ya, double za, double xb, double yb, double zb)[] Collisions => [];
        public EnumType Type = EnumType.Normal;
        public EnumFacing Facing = EnumFacing.North;
        public BlockMovingPiston()
        {
            
        }
        public override BlockMovingPiston Clone()
        {
            return new()
            {
                Type = Type,
                Facing = Facing
            };
        }
        public override Block Break()
        {
            return new BlockAir();
        }
    }
}
