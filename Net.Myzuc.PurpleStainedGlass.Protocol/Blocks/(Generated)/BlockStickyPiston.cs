namespace Net.Myzuc.PurpleStainedGlass.Protocol.Blocks
{
    public sealed class BlockStickyPiston : Block
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
                (0, 0, 0.25, 1, 1, 1)
            ],
            [
                (0, 0, 0, 0.75, 1, 1)
            ],
            [
                (0, 0, 0, 1, 1, 0.75)
            ],
            [
                (0.25, 0, 0, 1, 1, 1)
            ],
            [
                (0, 0, 0, 1, 0.75, 1)
            ],
            [
                (0, 0.25, 0, 1, 1, 1)
            ],
            [
                (0, 0, 0, 1, 1, 1)
            ],
            [
                (0, 0, 0, 1, 1, 1)
            ],
            [
                (0, 0, 0, 1, 1, 1)
            ],
            [
                (0, 0, 0, 1, 1, 1)
            ],
            [
                (0, 0, 0, 1, 1, 1)
            ],
            [
                (0, 0, 0, 1, 1, 1)
            ]
        ];
        public override int BlockId => 1992 + (int)Facing * 1 + (Extended ? 0 : 6);
        public override int LiquidId => 0;
        public override int LightEmission => 0;
        public override int LightFilter => 15;
        public override (double xa, double ya, double za, double xb, double yb, double zb)[] Collisions => [.. InternalCollisions[BlockId - 1992]];
        public EnumFacing Facing = EnumFacing.North;
        public bool Extended = false;
        public BlockStickyPiston()
        {
            
        }
        public override BlockStickyPiston Clone()
        {
            return new()
            {
                Facing = Facing,
                Extended = Extended
            };
        }
        public override Block Break()
        {
            return new BlockAir();
        }
    }
}
