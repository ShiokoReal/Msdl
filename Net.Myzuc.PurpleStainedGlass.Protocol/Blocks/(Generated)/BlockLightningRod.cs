namespace Net.Myzuc.PurpleStainedGlass.Protocol.Blocks
{
    public sealed class BlockLightningRod : Block
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
                (0.375, 0.375, 0, 0.625, 0.625, 1)
            ],
            [
                (0.375, 0.375, 0, 0.625, 0.625, 1)
            ],
            [
                (0.375, 0.375, 0, 0.625, 0.625, 1)
            ],
            [
                (0, 0.375, 0.375, 1, 0.625, 0.625)
            ],
            [
                (0, 0.375, 0.375, 1, 0.625, 0.625)
            ],
            [
                (0, 0.375, 0.375, 1, 0.625, 0.625)
            ],
            [
                (0, 0.375, 0.375, 1, 0.625, 0.625)
            ],
            [
                (0.375, 0.375, 0, 0.625, 0.625, 1)
            ],
            [
                (0.375, 0.375, 0, 0.625, 0.625, 1)
            ],
            [
                (0.375, 0.375, 0, 0.625, 0.625, 1)
            ],
            [
                (0.375, 0.375, 0, 0.625, 0.625, 1)
            ],
            [
                (0, 0.375, 0.375, 1, 0.625, 0.625)
            ],
            [
                (0, 0.375, 0.375, 1, 0.625, 0.625)
            ],
            [
                (0, 0.375, 0.375, 1, 0.625, 0.625)
            ],
            [
                (0, 0.375, 0.375, 1, 0.625, 0.625)
            ],
            [
                (0.375, 0, 0.375, 0.625, 1, 0.625)
            ],
            [
                (0.375, 0, 0.375, 0.625, 1, 0.625)
            ],
            [
                (0.375, 0, 0.375, 0.625, 1, 0.625)
            ],
            [
                (0.375, 0, 0.375, 0.625, 1, 0.625)
            ],
            [
                (0.375, 0, 0.375, 0.625, 1, 0.625)
            ],
            [
                (0.375, 0, 0.375, 0.625, 1, 0.625)
            ],
            [
                (0.375, 0, 0.375, 0.625, 1, 0.625)
            ],
            [
                (0.375, 0, 0.375, 0.625, 1, 0.625)
            ]
        ];
        public override int BlockId => 24724 + (Waterlogged ? 0 : 1) + (Powered ? 0 : 2) + (int)Facing * 4;
        public override int LiquidId => Waterlogged ? 1 : 0;
        public override int LightEmission => 0;
        public override int LightFilter => 0;
        public override (double xa, double ya, double za, double xb, double yb, double zb)[] Collisions => [.. InternalCollisions[BlockId - 24724]];
        public bool Waterlogged = false;
        public bool Powered = false;
        public EnumFacing Facing = EnumFacing.North;
        public BlockLightningRod()
        {
            
        }
        public override BlockLightningRod Clone()
        {
            return new()
            {
                Waterlogged = Waterlogged,
                Powered = Powered,
                Facing = Facing
            };
        }
        public override Block Break()
        {
            return Waterlogged ? new BlockWater() : new BlockAir();
        }
    }
}
