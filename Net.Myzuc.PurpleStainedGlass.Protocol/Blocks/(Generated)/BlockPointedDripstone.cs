namespace Net.Myzuc.PurpleStainedGlass.Protocol.Blocks
{
    public sealed class BlockPointedDripstone : Block
    {
        public enum EnumVerticalDirection : int
        {
            Up = 0,
            Down = 1
        }
        public enum EnumThickness : int
        {
            TipMerge = 0,
            Tip = 1,
            Frustum = 2,
            Middle = 3,
            Base = 4
        }
        private static (double xa, double ya, double za, double xb, double yb, double zb)[][] InternalCollisions = [
            [
                (0.1875, 0, 0.1875, 0.5625, 1, 0.5625)
            ],
            [
                (0.1875, 0, 0.1875, 0.5625, 1, 0.5625)
            ],
            [
                (0.1875, 0, 0.1875, 0.5625, 1, 0.5625)
            ],
            [
                (0.1875, 0, 0.1875, 0.5625, 1, 0.5625)
            ],
            [
                (0.1875, 0, 0.1875, 0.5625, 0.6875, 0.5625)
            ],
            [
                (0.1875, 0, 0.1875, 0.5625, 0.6875, 0.5625)
            ],
            [
                (0.1875, 0.3125, 0.1875, 0.5625, 1, 0.5625)
            ],
            [
                (0.1875, 0.3125, 0.1875, 0.5625, 1, 0.5625)
            ],
            [
                (0.125, 0, 0.125, 0.625, 1, 0.625)
            ],
            [
                (0.125, 0, 0.125, 0.625, 1, 0.625)
            ],
            [
                (0.125, 0, 0.125, 0.625, 1, 0.625)
            ],
            [
                (0.125, 0, 0.125, 0.625, 1, 0.625)
            ],
            [
                (0.0625, 0, 0.0625, 0.6875, 1, 0.6875)
            ],
            [
                (0.0625, 0, 0.0625, 0.6875, 1, 0.6875)
            ],
            [
                (0.0625, 0, 0.0625, 0.6875, 1, 0.6875)
            ],
            [
                (0.0625, 0, 0.0625, 0.6875, 1, 0.6875)
            ],
            [
                (0, 0, 0, 0.75, 1, 0.75)
            ],
            [
                (0, 0, 0, 0.75, 1, 0.75)
            ],
            [
                (0, 0, 0, 0.75, 1, 0.75)
            ],
            [
                (0, 0, 0, 0.75, 1, 0.75)
            ]
        ];
        public override int BlockId => 24748 + (Waterlogged ? 0 : 1) + (int)VerticalDirection * 2 + (int)Thickness * 4;
        public override int LiquidId => Waterlogged ? 1 : 0;
        public override int LightEmission => 0;
        public override int LightFilter => 0;
        public override (double xa, double ya, double za, double xb, double yb, double zb)[] Collisions => [.. InternalCollisions[BlockId - 24748]];
        public bool Waterlogged = false;
        public EnumVerticalDirection VerticalDirection = EnumVerticalDirection.Up;
        public EnumThickness Thickness = EnumThickness.TipMerge;
        public BlockPointedDripstone()
        {
            
        }
        public override BlockPointedDripstone Clone()
        {
            return new()
            {
                Waterlogged = Waterlogged,
                VerticalDirection = VerticalDirection,
                Thickness = Thickness
            };
        }
        public override Block Break()
        {
            return Waterlogged ? new BlockWater() : new BlockAir();
        }
    }
}
