namespace Net.Myzuc.PurpleStainedGlass.Protocol.Blocks
{
    public sealed class BlockLectern : Block
    {
        public enum EnumFacing : int
        {
            North = 0,
            South = 1,
            West = 2,
            East = 3
        }
        public override int BlockId => 18450 + (Powered ? 0 : 1) + (HasBook ? 0 : 2) + (int)Facing * 4;
        public override int LiquidId => 0;
        public override int LightEmission => 0;
        public override int LightFilter => 0;
        public override (double xa, double ya, double za, double xb, double yb, double zb)[] Collisions => [
            (0, 0, 0, 1, 0.125, 1),
            (0.25, 0.125, 0.25, 0.75, 0.875, 0.75)
        ];
        public bool Powered = false;
        public bool HasBook = false;
        public EnumFacing Facing = EnumFacing.North;
        public BlockLectern()
        {
            
        }
        public override BlockLectern Clone()
        {
            return new()
            {
                Powered = Powered,
                HasBook = HasBook,
                Facing = Facing
            };
        }
        public override Block Break()
        {
            return new BlockAir();
        }
    }
}
