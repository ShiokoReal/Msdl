namespace Net.Myzuc.PurpleStainedGlass.Protocol.Blocks
{
    public sealed class BlockLadder : Block
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
                (0, 0, 0.8125, 1, 1, 1)
            ],
            [
                (0, 0, 0.8125, 1, 1, 1)
            ],
            [
                (0, 0, 0, 1, 1, 0.1875)
            ],
            [
                (0, 0, 0, 1, 1, 0.1875)
            ],
            [
                (0.8125, 0, 0, 1, 1, 1)
            ],
            [
                (0.8125, 0, 0, 1, 1, 1)
            ],
            [
                (0, 0, 0, 0.1875, 1, 1)
            ],
            [
                (0, 0, 0, 0.1875, 1, 1)
            ]
        ];
        public override int BlockId => 4654 + (Waterlogged ? 0 : 1) + (int)Facing * 2;
        public override int LiquidId => Waterlogged ? 1 : 0;
        public override int LightEmission => 0;
        public override int LightFilter => 0;
        public override (double xa, double ya, double za, double xb, double yb, double zb)[] Collisions => [.. InternalCollisions[BlockId - 4654]];
        public bool Waterlogged = false;
        public EnumFacing Facing = EnumFacing.North;
        public BlockLadder()
        {
            
        }
        public override BlockLadder Clone()
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
