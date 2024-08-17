namespace Net.Myzuc.PurpleStainedGlass.Protocol.Blocks
{
    public sealed class BlockAmethystCluster : Block
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
                (0.1875, 0.1875, 0.5625, 0.8125, 0.8125, 1)
            ],
            [
                (0.1875, 0.1875, 0.5625, 0.8125, 0.8125, 1)
            ],
            [
                (0, 0.1875, 0.1875, 0.4375, 0.8125, 0.8125)
            ],
            [
                (0, 0.1875, 0.1875, 0.4375, 0.8125, 0.8125)
            ],
            [
                (0.1875, 0.1875, 0, 0.8125, 0.8125, 0.4375)
            ],
            [
                (0.1875, 0.1875, 0, 0.8125, 0.8125, 0.4375)
            ],
            [
                (0.5625, 0.1875, 0.1875, 1, 0.8125, 0.8125)
            ],
            [
                (0.5625, 0.1875, 0.1875, 1, 0.8125, 0.8125)
            ],
            [
                (0.1875, 0, 0.1875, 0.8125, 0.4375, 0.8125)
            ],
            [
                (0.1875, 0, 0.1875, 0.8125, 0.4375, 0.8125)
            ],
            [
                (0.1875, 0.5625, 0.1875, 0.8125, 1, 0.8125)
            ],
            [
                (0.1875, 0.5625, 0.1875, 0.8125, 1, 0.8125)
            ]
        ];
        public override int BlockId => 21033 + (Waterlogged ? 0 : 1) + (int)Facing * 2;
        public override int LiquidId => Waterlogged ? 1 : 0;
        public override int LightEmission => 5;
        public override int LightFilter => 0;
        public override (double xa, double ya, double za, double xb, double yb, double zb)[] Collisions => [.. InternalCollisions[BlockId - 21033]];
        public bool Waterlogged = false;
        public EnumFacing Facing = EnumFacing.North;
        public BlockAmethystCluster()
        {
            
        }
        public override BlockAmethystCluster Clone()
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
