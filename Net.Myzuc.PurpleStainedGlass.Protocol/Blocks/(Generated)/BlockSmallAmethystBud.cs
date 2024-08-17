namespace Net.Myzuc.PurpleStainedGlass.Protocol.Blocks
{
    public sealed class BlockSmallAmethystBud : Block
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
                (0.25, 0.25, 0.8125, 0.75, 0.75, 1)
            ],
            [
                (0.25, 0.25, 0.8125, 0.75, 0.75, 1)
            ],
            [
                (0, 0.25, 0.25, 0.1875, 0.75, 0.75)
            ],
            [
                (0, 0.25, 0.25, 0.1875, 0.75, 0.75)
            ],
            [
                (0.25, 0.25, 0, 0.75, 0.75, 0.1875)
            ],
            [
                (0.25, 0.25, 0, 0.75, 0.75, 0.1875)
            ],
            [
                (0.8125, 0.25, 0.25, 1, 0.75, 0.75)
            ],
            [
                (0.8125, 0.25, 0.25, 1, 0.75, 0.75)
            ],
            [
                (0.25, 0, 0.25, 0.75, 0.1875, 0.75)
            ],
            [
                (0.25, 0, 0.25, 0.75, 0.1875, 0.75)
            ],
            [
                (0.25, 0.8125, 0.25, 0.75, 1, 0.75)
            ],
            [
                (0.25, 0.8125, 0.25, 0.75, 1, 0.75)
            ]
        ];
        public override int BlockId => 21069 + (Waterlogged ? 0 : 1) + (int)Facing * 2;
        public override int LiquidId => Waterlogged ? 1 : 0;
        public override int LightEmission => 1;
        public override int LightFilter => 0;
        public override (double xa, double ya, double za, double xb, double yb, double zb)[] Collisions => [.. InternalCollisions[BlockId - 21069]];
        public bool Waterlogged = false;
        public EnumFacing Facing = EnumFacing.North;
        public BlockSmallAmethystBud()
        {
            
        }
        public override BlockSmallAmethystBud Clone()
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
