namespace Net.Myzuc.PurpleStainedGlass.Protocol.Blocks
{
    public sealed class BlockCommandBlock : Block
    {
        public enum EnumFacing : int
        {
            North = 0,
            East = 1,
            South = 2,
            West = 3,
            Up = 4,
            Down = 5
        }
        public override int BlockId => 7906 + (int)Facing * 1 + (Conditional ? 0 : 6);
        public override int LiquidId => 0;
        public override int LightEmission => 0;
        public override int LightFilter => 15;
        public override (double xa, double ya, double za, double xb, double yb, double zb)[] Collisions => [
            (0, 0, 0, 1, 1, 1)
        ];
        public EnumFacing Facing = EnumFacing.North;
        public bool Conditional = false;
        public BlockCommandBlock()
        {
            
        }
        public override BlockCommandBlock Clone()
        {
            return new()
            {
                Facing = Facing,
                Conditional = Conditional
            };
        }
        public override Block Break()
        {
            return new BlockAir();
        }
    }
}
