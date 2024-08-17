namespace Net.Myzuc.PurpleStainedGlass.Protocol.Blocks
{
    public sealed class BlockEndRod : Block
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
        private static (double xa, double ya, double za, double xb, double yb, double zb)[][] InternalCollisions = [
            [
                (0.375, 0.375, 0, 0.625, 0.625, 1)
            ],
            [
                (0, 0.375, 0.375, 1, 0.625, 0.625)
            ],
            [
                (0.375, 0.375, 0, 0.625, 0.625, 1)
            ],
            [
                (0, 0.375, 0.375, 1, 0.625, 0.625)
            ],
            [
                (0.375, 0, 0.375, 0.625, 1, 0.625)
            ],
            [
                (0.375, 0, 0.375, 0.625, 1, 0.625)
            ]
        ];
        public override int BlockId => 12334 + (int)Facing * 1;
        public override int LiquidId => 0;
        public override int LightEmission => 14;
        public override int LightFilter => 0;
        public override (double xa, double ya, double za, double xb, double yb, double zb)[] Collisions => [.. InternalCollisions[BlockId - 12334]];
        public EnumFacing Facing = EnumFacing.North;
        public BlockEndRod()
        {
            
        }
        public override BlockEndRod Clone()
        {
            return new()
            {
                Facing = Facing
            };
        }
        public override Block Break()
        {
            return new BlockAir();
        }
    }
}
