namespace Net.Myzuc.PurpleStainedGlass.Protocol.Blocks
{
    public sealed class BlockCherryFenceGate : Block
    {
        public enum EnumFacing : int
        {
            North = 0,
            South = 1,
            West = 2,
            East = 3
        }
        private static (double xa, double ya, double za, double xb, double yb, double zb)[][] InternalCollisions = [
            [],
            [],
            [
                (0, 0, 0.375, 1, 1.5, 0.625)
            ],
            [
                (0, 0, 0.375, 1, 1.5, 0.625)
            ],
            [],
            [],
            [
                (0, 0, 0.375, 1, 1.5, 0.625)
            ],
            [
                (0, 0, 0.375, 1, 1.5, 0.625)
            ],
            [],
            [],
            [
                (0, 0, 0.375, 1, 1.5, 0.625)
            ],
            [
                (0, 0, 0.375, 1, 1.5, 0.625)
            ],
            [],
            [],
            [
                (0, 0, 0.375, 1, 1.5, 0.625)
            ],
            [
                (0, 0, 0.375, 1, 1.5, 0.625)
            ],
            [],
            [],
            [
                (0.375, 0, 0, 0.625, 1.5, 1)
            ],
            [
                (0.375, 0, 0, 0.625, 1.5, 1)
            ],
            [],
            [],
            [
                (0.375, 0, 0, 0.625, 1.5, 1)
            ],
            [
                (0.375, 0, 0, 0.625, 1.5, 1)
            ],
            [],
            [],
            [
                (0.375, 0, 0, 0.625, 1.5, 1)
            ],
            [
                (0.375, 0, 0, 0.625, 1.5, 1)
            ],
            [],
            [],
            [
                (0.375, 0, 0, 0.625, 1.5, 1)
            ],
            [
                (0.375, 0, 0, 0.625, 1.5, 1)
            ]
        ];
        public override int BlockId => 11438 + (Powered ? 0 : 1) + (Open ? 0 : 2) + (InWall ? 0 : 4) + (int)Facing * 8;
        public override int LiquidId => 0;
        public override int LightEmission => 0;
        public override int LightFilter => 0;
        public override (double xa, double ya, double za, double xb, double yb, double zb)[] Collisions => [.. InternalCollisions[BlockId - 11438]];
        public bool Powered = false;
        public bool Open = false;
        public bool InWall = false;
        public EnumFacing Facing = EnumFacing.North;
        public BlockCherryFenceGate()
        {
            
        }
        public override BlockCherryFenceGate Clone()
        {
            return new()
            {
                Powered = Powered,
                Open = Open,
                InWall = InWall,
                Facing = Facing
            };
        }
        public override Block Break()
        {
            return new BlockAir();
        }
    }
}
