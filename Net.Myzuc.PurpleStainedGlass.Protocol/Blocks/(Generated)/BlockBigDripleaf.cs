namespace Net.Myzuc.PurpleStainedGlass.Protocol.Blocks
{
    public sealed class BlockBigDripleaf : Block
    {
        public enum EnumTilt : int
        {
            None = 0,
            Unstable = 1,
            Partial = 2,
            Full = 3
        }
        public enum EnumFacing : int
        {
            North = 0,
            South = 1,
            West = 2,
            East = 3
        }
        private static (double xa, double ya, double za, double xb, double yb, double zb)[][] InternalCollisions = [
            [
                (0, 0.6875, 0, 1, 0.9375, 1)
            ],
            [
                (0, 0.6875, 0, 1, 0.9375, 1)
            ],
            [
                (0, 0.6875, 0, 1, 0.9375, 1)
            ],
            [
                (0, 0.6875, 0, 1, 0.9375, 1)
            ],
            [
                (0, 0.6875, 0, 1, 0.8125, 1)
            ],
            [
                (0, 0.6875, 0, 1, 0.8125, 1)
            ],
            [],
            [],
            [
                (0, 0.6875, 0, 1, 0.9375, 1)
            ],
            [
                (0, 0.6875, 0, 1, 0.9375, 1)
            ],
            [
                (0, 0.6875, 0, 1, 0.9375, 1)
            ],
            [
                (0, 0.6875, 0, 1, 0.9375, 1)
            ],
            [
                (0, 0.6875, 0, 1, 0.8125, 1)
            ],
            [
                (0, 0.6875, 0, 1, 0.8125, 1)
            ],
            [],
            [],
            [
                (0, 0.6875, 0, 1, 0.9375, 1)
            ],
            [
                (0, 0.6875, 0, 1, 0.9375, 1)
            ],
            [
                (0, 0.6875, 0, 1, 0.9375, 1)
            ],
            [
                (0, 0.6875, 0, 1, 0.9375, 1)
            ],
            [
                (0, 0.6875, 0, 1, 0.8125, 1)
            ],
            [
                (0, 0.6875, 0, 1, 0.8125, 1)
            ],
            [],
            [],
            [
                (0, 0.6875, 0, 1, 0.9375, 1)
            ],
            [
                (0, 0.6875, 0, 1, 0.9375, 1)
            ],
            [
                (0, 0.6875, 0, 1, 0.9375, 1)
            ],
            [
                (0, 0.6875, 0, 1, 0.9375, 1)
            ],
            [
                (0, 0.6875, 0, 1, 0.8125, 1)
            ],
            [
                (0, 0.6875, 0, 1, 0.8125, 1)
            ],
            [],
            []
        ];
        public override int BlockId => 24844 + (Waterlogged ? 0 : 1) + (int)Tilt * 2 + (int)Facing * 8;
        public override int LiquidId => Waterlogged ? 1 : 0;
        public override int LightEmission => 0;
        public override int LightFilter => 0;
        public override (double xa, double ya, double za, double xb, double yb, double zb)[] Collisions => [.. InternalCollisions[BlockId - 24844]];
        public bool Waterlogged = false;
        public EnumTilt Tilt = EnumTilt.None;
        public EnumFacing Facing = EnumFacing.North;
        public BlockBigDripleaf()
        {
            
        }
        public override BlockBigDripleaf Clone()
        {
            return new()
            {
                Waterlogged = Waterlogged,
                Tilt = Tilt,
                Facing = Facing
            };
        }
        public override Block Break()
        {
            return Waterlogged ? new BlockWater() : new BlockAir();
        }
    }
}
