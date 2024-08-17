namespace Net.Myzuc.PurpleStainedGlass.Protocol.Blocks
{
    public sealed class BlockSoulLantern : Block
    {
        private static (double xa, double ya, double za, double xb, double yb, double zb)[][] InternalCollisions = [
            [
                (0.3125, 0.0625, 0.3125, 0.6875, 0.5, 0.6875),
                (0.375, 0.5, 0.375, 0.625, 0.625, 0.625)
            ],
            [
                (0.3125, 0.0625, 0.3125, 0.6875, 0.5, 0.6875),
                (0.375, 0.5, 0.375, 0.625, 0.625, 0.625)
            ],
            [
                (0.3125, 0, 0.3125, 0.6875, 0.4375, 0.6875),
                (0.375, 0.4375, 0.375, 0.625, 0.5625, 0.625)
            ],
            [
                (0.3125, 0, 0.3125, 0.6875, 0.4375, 0.6875),
                (0.375, 0.4375, 0.375, 0.625, 0.5625, 0.625)
            ]
        ];
        public override int BlockId => 18507 + (Waterlogged ? 0 : 1) + (Hanging ? 0 : 2);
        public override int LiquidId => Waterlogged ? 1 : 0;
        public override int LightEmission => 10;
        public override int LightFilter => 0;
        public override (double xa, double ya, double za, double xb, double yb, double zb)[] Collisions => [.. InternalCollisions[BlockId - 18507]];
        public bool Waterlogged = false;
        public bool Hanging = false;
        public BlockSoulLantern()
        {
            
        }
        public override BlockSoulLantern Clone()
        {
            return new()
            {
                Waterlogged = Waterlogged,
                Hanging = Hanging
            };
        }
        public override Block Break()
        {
            return Waterlogged ? new BlockWater() : new BlockAir();
        }
    }
}
