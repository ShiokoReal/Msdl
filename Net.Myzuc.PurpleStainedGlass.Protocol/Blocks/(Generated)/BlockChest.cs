namespace Net.Myzuc.PurpleStainedGlass.Protocol.Blocks
{
    public sealed class BlockChest : Block
    {
        public enum EnumType : int
        {
            Single = 0,
            Left = 1,
            Right = 2
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
                (0.0625, 0, 0.0625, 0.9375, 0.875, 0.9375)
            ],
            [
                (0.0625, 0, 0.0625, 0.9375, 0.875, 0.9375)
            ],
            [
                (0.0625, 0, 0.0625, 1, 0.875, 0.9375)
            ],
            [
                (0.0625, 0, 0.0625, 1, 0.875, 0.9375)
            ],
            [
                (0, 0, 0.0625, 0.9375, 0.875, 0.9375)
            ],
            [
                (0, 0, 0.0625, 0.9375, 0.875, 0.9375)
            ],
            [
                (0.0625, 0, 0.0625, 0.9375, 0.875, 0.9375)
            ],
            [
                (0.0625, 0, 0.0625, 0.9375, 0.875, 0.9375)
            ],
            [
                (0, 0, 0.0625, 0.9375, 0.875, 0.9375)
            ],
            [
                (0, 0, 0.0625, 0.9375, 0.875, 0.9375)
            ],
            [
                (0.0625, 0, 0.0625, 1, 0.875, 0.9375)
            ],
            [
                (0.0625, 0, 0.0625, 1, 0.875, 0.9375)
            ],
            [
                (0.0625, 0, 0.0625, 0.9375, 0.875, 0.9375)
            ],
            [
                (0.0625, 0, 0.0625, 0.9375, 0.875, 0.9375)
            ],
            [
                (0.0625, 0, 0, 0.9375, 0.875, 0.9375)
            ],
            [
                (0.0625, 0, 0, 0.9375, 0.875, 0.9375)
            ],
            [
                (0.0625, 0, 0.0625, 0.9375, 0.875, 1)
            ],
            [
                (0.0625, 0, 0.0625, 0.9375, 0.875, 1)
            ],
            [
                (0.0625, 0, 0.0625, 0.9375, 0.875, 0.9375)
            ],
            [
                (0.0625, 0, 0.0625, 0.9375, 0.875, 0.9375)
            ],
            [
                (0.0625, 0, 0.0625, 0.9375, 0.875, 1)
            ],
            [
                (0.0625, 0, 0.0625, 0.9375, 0.875, 1)
            ],
            [
                (0.0625, 0, 0, 0.9375, 0.875, 0.9375)
            ],
            [
                (0.0625, 0, 0, 0.9375, 0.875, 0.9375)
            ]
        ];
        public override int BlockId => 2954 + (Waterlogged ? 0 : 1) + (int)Type * 2 + (int)Facing * 6;
        public override int LiquidId => Waterlogged ? 1 : 0;
        public override int LightEmission => 0;
        public override int LightFilter => 0;
        public override (double xa, double ya, double za, double xb, double yb, double zb)[] Collisions => [.. InternalCollisions[BlockId - 2954]];
        public bool Waterlogged = false;
        public EnumType Type = EnumType.Single;
        public EnumFacing Facing = EnumFacing.North;
        public BlockChest()
        {
            
        }
        public override BlockChest Clone()
        {
            return new()
            {
                Waterlogged = Waterlogged,
                Type = Type,
                Facing = Facing
            };
        }
        public override Block Break()
        {
            return Waterlogged ? new BlockWater() : new BlockAir();
        }
    }
}
