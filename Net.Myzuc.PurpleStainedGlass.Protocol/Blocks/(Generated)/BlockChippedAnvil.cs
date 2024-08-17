namespace Net.Myzuc.PurpleStainedGlass.Protocol.Blocks
{
    public sealed class BlockChippedAnvil : Block
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
                (0.125, 0, 0.125, 0.875, 0.25, 0.875),
                (0.25, 0.25, 0.1875, 0.75, 0.3125, 0.8125),
                (0.375, 0.3125, 0.25, 0.625, 1, 0.75),
                (0.1875, 0.625, 0, 0.375, 1, 1),
                (0.375, 0.625, 0, 0.8125, 1, 0.25),
                (0.375, 0.625, 0.75, 0.8125, 1, 1),
                (0.625, 0.625, 0.25, 0.8125, 1, 0.75)
            ],
            [
                (0.125, 0, 0.125, 0.875, 0.25, 0.875),
                (0.25, 0.25, 0.1875, 0.75, 0.3125, 0.8125),
                (0.375, 0.3125, 0.25, 0.625, 1, 0.75),
                (0.1875, 0.625, 0, 0.375, 1, 1),
                (0.375, 0.625, 0, 0.8125, 1, 0.25),
                (0.375, 0.625, 0.75, 0.8125, 1, 1),
                (0.625, 0.625, 0.25, 0.8125, 1, 0.75)
            ],
            [
                (0.125, 0, 0.125, 0.875, 0.25, 0.875),
                (0.1875, 0.25, 0.25, 0.8125, 0.3125, 0.75),
                (0.25, 0.3125, 0.375, 0.75, 1, 0.625),
                (0, 0.625, 0.1875, 0.25, 1, 0.8125),
                (0.25, 0.625, 0.1875, 1, 1, 0.375),
                (0.25, 0.625, 0.625, 1, 1, 0.8125),
                (0.75, 0.625, 0.375, 1, 1, 0.625)
            ],
            [
                (0.125, 0, 0.125, 0.875, 0.25, 0.875),
                (0.1875, 0.25, 0.25, 0.8125, 0.3125, 0.75),
                (0.25, 0.3125, 0.375, 0.75, 1, 0.625),
                (0, 0.625, 0.1875, 0.25, 1, 0.8125),
                (0.25, 0.625, 0.1875, 1, 1, 0.375),
                (0.25, 0.625, 0.625, 1, 1, 0.8125),
                (0.75, 0.625, 0.375, 1, 1, 0.625)
            ]
        ];
        public override int BlockId => 9111 + (int)Facing * 1;
        public override int LiquidId => 0;
        public override int LightEmission => 0;
        public override int LightFilter => 0;
        public override (double xa, double ya, double za, double xb, double yb, double zb)[] Collisions => [.. InternalCollisions[BlockId - 9111]];
        public EnumFacing Facing = EnumFacing.North;
        public BlockChippedAnvil()
        {
            
        }
        public override BlockChippedAnvil Clone()
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
