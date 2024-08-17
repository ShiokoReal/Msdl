namespace Net.Myzuc.PurpleStainedGlass.Protocol.Blocks
{
    public sealed class BlockCrimsonWallHangingSign : Block
    {
        public enum EnumFacing : int
        {
            North = 0,
            South = 1,
            West = 2,
            East = 3
        }
        private static (double xa, double ya, double za, double xb, double yb, double zb)[][] InternalCollisions = [
            [
                (0, 0.875, 0.375, 1, 1, 0.625)
            ],
            [
                (0, 0.875, 0.375, 1, 1, 0.625)
            ],
            [
                (0, 0.875, 0.375, 1, 1, 0.625)
            ],
            [
                (0, 0.875, 0.375, 1, 1, 0.625)
            ],
            [
                (0.375, 0.875, 0, 0.625, 1, 1)
            ],
            [
                (0.375, 0.875, 0, 0.625, 1, 1)
            ],
            [
                (0.375, 0.875, 0, 0.625, 1, 1)
            ],
            [
                (0.375, 0.875, 0, 0.625, 1, 1)
            ]
        ];
        public override int BlockId => 5602 + (Waterlogged ? 0 : 1) + (int)Facing * 2;
        public override int LiquidId => Waterlogged ? 1 : 0;
        public override int LightEmission => 0;
        public override int LightFilter => 0;
        public override (double xa, double ya, double za, double xb, double yb, double zb)[] Collisions => [.. InternalCollisions[BlockId - 5602]];
        public bool Waterlogged = false;
        public EnumFacing Facing = EnumFacing.North;
        public BlockCrimsonWallHangingSign()
        {
            
        }
        public override BlockCrimsonWallHangingSign Clone()
        {
            return new()
            {
                Waterlogged = Waterlogged,
                Facing = Facing
            };
        }
        public override Block Break()
        {
            return Waterlogged ? new BlockWater() : new BlockAir();
        }
    }
}
