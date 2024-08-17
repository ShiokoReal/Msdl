namespace Net.Myzuc.PurpleStainedGlass.Protocol.Blocks
{
    public sealed class BlockComparator : Block
    {
        public enum EnumMode : int
        {
            Compare = 0,
            Subtract = 1
        }
        public enum EnumFacing : int
        {
            North = 0,
            South = 1,
            West = 2,
            East = 3
        }
        public override int BlockId => 9175 + (Powered ? 0 : 1) + (int)Mode * 2 + (int)Facing * 4;
        public override int LiquidId => 0;
        public override int LightEmission => 0;
        public override int LightFilter => 0;
        public override (double xa, double ya, double za, double xb, double yb, double zb)[] Collisions => [
            (0, 0, 0, 1, 0.125, 1)
        ];
        public bool Powered = false;
        public EnumMode Mode = EnumMode.Compare;
        public EnumFacing Facing = EnumFacing.North;
        public BlockComparator()
        {
            
        }
        public override BlockComparator Clone()
        {
            return new()
            {
                Powered = Powered,
                Mode = Mode,
                Facing = Facing
            };
        }
        public override Block Break()
        {
            return new BlockAir();
        }
    }
}
