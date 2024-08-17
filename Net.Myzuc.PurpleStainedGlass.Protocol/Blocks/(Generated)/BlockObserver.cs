namespace Net.Myzuc.PurpleStainedGlass.Protocol.Blocks
{
    public sealed class BlockObserver : Block
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
        public override int BlockId => 12550 + (Powered ? 0 : 1) + (int)Facing * 2;
        public override int LiquidId => 0;
        public override int LightEmission => 0;
        public override int LightFilter => 15;
        public override (double xa, double ya, double za, double xb, double yb, double zb)[] Collisions => [
            (0, 0, 0, 1, 1, 1)
        ];
        public bool Powered = false;
        public EnumFacing Facing = EnumFacing.North;
        public BlockObserver()
        {
            
        }
        public override BlockObserver Clone()
        {
            return new()
            {
                Powered = Powered,
                Facing = Facing
            };
        }
        public override Block Break()
        {
            return new BlockAir();
        }
    }
}
