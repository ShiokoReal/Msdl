namespace Net.Myzuc.PurpleStainedGlass.Protocol.Blocks
{
    public sealed class BlockDarkOakTrapdoor : Block
    {
        public enum EnumHalf : int
        {
            Top = 0,
            Bottom = 1
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
                (0, 0, 0.8125, 1, 1, 1)
            ],
            [
                (0, 0, 0.8125, 1, 1, 1)
            ],
            [
                (0, 0, 0.8125, 1, 1, 1)
            ],
            [
                (0, 0, 0.8125, 1, 1, 1)
            ],
            [
                (0, 0.8125, 0, 1, 1, 1)
            ],
            [
                (0, 0.8125, 0, 1, 1, 1)
            ],
            [
                (0, 0.8125, 0, 1, 1, 1)
            ],
            [
                (0, 0.8125, 0, 1, 1, 1)
            ],
            [
                (0, 0, 0.8125, 1, 1, 1)
            ],
            [
                (0, 0, 0.8125, 1, 1, 1)
            ],
            [
                (0, 0, 0.8125, 1, 1, 1)
            ],
            [
                (0, 0, 0.8125, 1, 1, 1)
            ],
            [
                (0, 0, 0, 1, 0.1875, 1)
            ],
            [
                (0, 0, 0, 1, 0.1875, 1)
            ],
            [
                (0, 0, 0, 1, 0.1875, 1)
            ],
            [
                (0, 0, 0, 1, 0.1875, 1)
            ],
            [
                (0, 0, 0, 1, 1, 0.1875)
            ],
            [
                (0, 0, 0, 1, 1, 0.1875)
            ],
            [
                (0, 0, 0, 1, 1, 0.1875)
            ],
            [
                (0, 0, 0, 1, 1, 0.1875)
            ],
            [
                (0, 0.8125, 0, 1, 1, 1)
            ],
            [
                (0, 0.8125, 0, 1, 1, 1)
            ],
            [
                (0, 0.8125, 0, 1, 1, 1)
            ],
            [
                (0, 0.8125, 0, 1, 1, 1)
            ],
            [
                (0, 0, 0, 1, 1, 0.1875)
            ],
            [
                (0, 0, 0, 1, 1, 0.1875)
            ],
            [
                (0, 0, 0, 1, 1, 0.1875)
            ],
            [
                (0, 0, 0, 1, 1, 0.1875)
            ],
            [
                (0, 0, 0, 1, 0.1875, 1)
            ],
            [
                (0, 0, 0, 1, 0.1875, 1)
            ],
            [
                (0, 0, 0, 1, 0.1875, 1)
            ],
            [
                (0, 0, 0, 1, 0.1875, 1)
            ],
            [
                (0.8125, 0, 0, 1, 1, 1)
            ],
            [
                (0.8125, 0, 0, 1, 1, 1)
            ],
            [
                (0.8125, 0, 0, 1, 1, 1)
            ],
            [
                (0.8125, 0, 0, 1, 1, 1)
            ],
            [
                (0, 0.8125, 0, 1, 1, 1)
            ],
            [
                (0, 0.8125, 0, 1, 1, 1)
            ],
            [
                (0, 0.8125, 0, 1, 1, 1)
            ],
            [
                (0, 0.8125, 0, 1, 1, 1)
            ],
            [
                (0.8125, 0, 0, 1, 1, 1)
            ],
            [
                (0.8125, 0, 0, 1, 1, 1)
            ],
            [
                (0.8125, 0, 0, 1, 1, 1)
            ],
            [
                (0.8125, 0, 0, 1, 1, 1)
            ],
            [
                (0, 0, 0, 1, 0.1875, 1)
            ],
            [
                (0, 0, 0, 1, 0.1875, 1)
            ],
            [
                (0, 0, 0, 1, 0.1875, 1)
            ],
            [
                (0, 0, 0, 1, 0.1875, 1)
            ],
            [
                (0, 0, 0, 0.1875, 1, 1)
            ],
            [
                (0, 0, 0, 0.1875, 1, 1)
            ],
            [
                (0, 0, 0, 0.1875, 1, 1)
            ],
            [
                (0, 0, 0, 0.1875, 1, 1)
            ],
            [
                (0, 0.8125, 0, 1, 1, 1)
            ],
            [
                (0, 0.8125, 0, 1, 1, 1)
            ],
            [
                (0, 0.8125, 0, 1, 1, 1)
            ],
            [
                (0, 0.8125, 0, 1, 1, 1)
            ],
            [
                (0, 0, 0, 0.1875, 1, 1)
            ],
            [
                (0, 0, 0, 0.1875, 1, 1)
            ],
            [
                (0, 0, 0, 0.1875, 1, 1)
            ],
            [
                (0, 0, 0, 0.1875, 1, 1)
            ],
            [
                (0, 0, 0, 1, 0.1875, 1)
            ],
            [
                (0, 0, 0, 1, 0.1875, 1)
            ],
            [
                (0, 0, 0, 1, 0.1875, 1)
            ],
            [
                (0, 0, 0, 1, 0.1875, 1)
            ]
        ];
        public override int BlockId => 6345 + (Waterlogged ? 0 : 1) + (Powered ? 0 : 2) + (Open ? 0 : 4) + (int)Half * 8 + (int)Facing * 16;
        public override int LiquidId => Waterlogged ? 1 : 0;
        public override int LightEmission => 0;
        public override int LightFilter => 0;
        public override (double xa, double ya, double za, double xb, double yb, double zb)[] Collisions => [.. InternalCollisions[BlockId - 6345]];
        public bool Waterlogged = false;
        public bool Powered = false;
        public bool Open = false;
        public EnumHalf Half = EnumHalf.Top;
        public EnumFacing Facing = EnumFacing.North;
        public BlockDarkOakTrapdoor()
        {
            
        }
        public override BlockDarkOakTrapdoor Clone()
        {
            return new()
            {
                Waterlogged = Waterlogged,
                Powered = Powered,
                Open = Open,
                Half = Half,
                Facing = Facing
            };
        }
        public override Block Break()
        {
            return Waterlogged ? new BlockWater() : new BlockAir();
        }
    }
}
