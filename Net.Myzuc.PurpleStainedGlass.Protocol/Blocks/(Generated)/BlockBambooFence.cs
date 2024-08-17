namespace Net.Myzuc.PurpleStainedGlass.Protocol.Blocks
{
    public sealed class BlockBambooFence : Block
    {
        private static (double xa, double ya, double za, double xb, double yb, double zb)[][] InternalCollisions = [
            [
                (0, 0, 0.375, 1, 1.5, 0.625),
                (0.375, 0, 0, 0.625, 1.5, 0.375),
                (0.375, 0, 0.625, 0.625, 1.5, 1)
            ],
            [
                (0.375, 0, 0, 0.625, 1.5, 1),
                (0.625, 0, 0.375, 1, 1.5, 0.625)
            ],
            [
                (0, 0, 0.375, 1, 1.5, 0.625),
                (0.375, 0, 0, 0.625, 1.5, 0.375),
                (0.375, 0, 0.625, 0.625, 1.5, 1)
            ],
            [
                (0.375, 0, 0, 0.625, 1.5, 1),
                (0.625, 0, 0.375, 1, 1.5, 0.625)
            ],
            [
                (0, 0, 0.375, 1, 1.5, 0.625),
                (0.375, 0, 0, 0.625, 1.5, 0.375)
            ],
            [
                (0.375, 0, 0, 0.625, 1.5, 0.625),
                (0.625, 0, 0.375, 1, 1.5, 0.625)
            ],
            [
                (0, 0, 0.375, 1, 1.5, 0.625),
                (0.375, 0, 0, 0.625, 1.5, 0.375)
            ],
            [
                (0.375, 0, 0, 0.625, 1.5, 0.625),
                (0.625, 0, 0.375, 1, 1.5, 0.625)
            ],
            [
                (0, 0, 0.375, 1, 1.5, 0.625),
                (0.375, 0, 0.625, 0.625, 1.5, 1)
            ],
            [
                (0.375, 0, 0.375, 0.625, 1.5, 1),
                (0.625, 0, 0.375, 1, 1.5, 0.625)
            ],
            [
                (0, 0, 0.375, 1, 1.5, 0.625),
                (0.375, 0, 0.625, 0.625, 1.5, 1)
            ],
            [
                (0.375, 0, 0.375, 0.625, 1.5, 1),
                (0.625, 0, 0.375, 1, 1.5, 0.625)
            ],
            [
                (0, 0, 0.375, 1, 1.5, 0.625)
            ],
            [
                (0.375, 0, 0.375, 1, 1.5, 0.625)
            ],
            [
                (0, 0, 0.375, 1, 1.5, 0.625)
            ],
            [
                (0.375, 0, 0.375, 1, 1.5, 0.625)
            ],
            [
                (0, 0, 0.375, 0.625, 1.5, 0.625),
                (0.375, 0, 0, 0.625, 1.5, 0.375),
                (0.375, 0, 0.625, 0.625, 1.5, 1)
            ],
            [
                (0.375, 0, 0, 0.625, 1.5, 1)
            ],
            [
                (0, 0, 0.375, 0.625, 1.5, 0.625),
                (0.375, 0, 0, 0.625, 1.5, 0.375),
                (0.375, 0, 0.625, 0.625, 1.5, 1)
            ],
            [
                (0.375, 0, 0, 0.625, 1.5, 1)
            ],
            [
                (0, 0, 0.375, 0.625, 1.5, 0.625),
                (0.375, 0, 0, 0.625, 1.5, 0.375)
            ],
            [
                (0.375, 0, 0, 0.625, 1.5, 0.625)
            ],
            [
                (0, 0, 0.375, 0.625, 1.5, 0.625),
                (0.375, 0, 0, 0.625, 1.5, 0.375)
            ],
            [
                (0.375, 0, 0, 0.625, 1.5, 0.625)
            ],
            [
                (0, 0, 0.375, 0.625, 1.5, 0.625),
                (0.375, 0, 0.625, 0.625, 1.5, 1)
            ],
            [
                (0.375, 0, 0.375, 0.625, 1.5, 1)
            ],
            [
                (0, 0, 0.375, 0.625, 1.5, 0.625),
                (0.375, 0, 0.625, 0.625, 1.5, 1)
            ],
            [
                (0.375, 0, 0.375, 0.625, 1.5, 1)
            ],
            [
                (0, 0, 0.375, 0.625, 1.5, 0.625)
            ],
            [
                (0.375, 0, 0.375, 0.625, 1.5, 0.625)
            ],
            [
                (0, 0, 0.375, 0.625, 1.5, 0.625)
            ],
            [
                (0.375, 0, 0.375, 0.625, 1.5, 0.625)
            ]
        ];
        public override int BlockId => 11790 + (West ? 0 : 1) + (Waterlogged ? 0 : 2) + (South ? 0 : 4) + (North ? 0 : 8) + (East ? 0 : 16);
        public override int LiquidId => Waterlogged ? 1 : 0;
        public override int LightEmission => 0;
        public override int LightFilter => 0;
        public override (double xa, double ya, double za, double xb, double yb, double zb)[] Collisions => [.. InternalCollisions[BlockId - 11790]];
        public bool West = false;
        public bool Waterlogged = false;
        public bool South = false;
        public bool North = false;
        public bool East = false;
        public BlockBambooFence()
        {
            
        }
        public override BlockBambooFence Clone()
        {
            return new()
            {
                West = West,
                Waterlogged = Waterlogged,
                South = South,
                North = North,
                East = East
            };
        }
        public override Block Break()
        {
            return Waterlogged ? new BlockWater() : new BlockAir();
        }
    }
}