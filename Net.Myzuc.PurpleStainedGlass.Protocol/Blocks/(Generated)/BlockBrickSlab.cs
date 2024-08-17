namespace Net.Myzuc.PurpleStainedGlass.Protocol.Blocks
{
    public sealed class BlockBrickSlab : Block
    {
        public enum EnumType : int
        {
            Top = 0,
            Bottom = 1,
            Double = 2
        }
        private static (double xa, double ya, double za, double xb, double yb, double zb)[][] InternalCollisions = [
            [
                (0, 0.5, 0, 1, 1, 1)
            ],
            [
                (0, 0.5, 0, 1, 1, 1)
            ],
            [
                (0, 0, 0, 1, 0.5, 1)
            ],
            [
                (0, 0, 0, 1, 0.5, 1)
            ],
            [
                (0, 0, 0, 1, 1, 1)
            ],
            [
                (0, 0, 0, 1, 1, 1)
            ]
        ];
        public override int BlockId => 11258 + (Waterlogged ? 0 : 1) + (int)Type * 2;
        public override int LiquidId => Waterlogged ? 1 : 0;
        public override int LightEmission => 0;
        public override int LightFilter => 0;
        public override (double xa, double ya, double za, double xb, double yb, double zb)[] Collisions => [.. InternalCollisions[BlockId - 11258]];
        public bool Waterlogged = false;
        public EnumType Type = EnumType.Top;
        public BlockBrickSlab()
        {
            
        }
        public override BlockBrickSlab Clone()
        {
            return new()
            {
                Waterlogged = Waterlogged,
                Type = Type
            };
        }
        public override Block Break()
        {
            return Waterlogged ? new BlockWater() : new BlockAir();
        }
    }
}
