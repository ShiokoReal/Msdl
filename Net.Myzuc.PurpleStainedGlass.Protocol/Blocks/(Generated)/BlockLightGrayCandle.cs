using System.ComponentModel.DataAnnotations;
namespace Net.Myzuc.PurpleStainedGlass.Protocol.Blocks
{
    public sealed class BlockLightGrayCandle : Block
    {
        private static (double xa, double ya, double za, double xb, double yb, double zb)[][] InternalCollisions = [
            [
                (0.4375, 0, 0.4375, 0.5625, 0.375, 0.5625)
            ],
            [
                (0.4375, 0, 0.4375, 0.5625, 0.375, 0.5625)
            ],
            [
                (0.4375, 0, 0.4375, 0.5625, 0.375, 0.5625)
            ],
            [
                (0.4375, 0, 0.4375, 0.5625, 0.375, 0.5625)
            ],
            [
                (0.3125, 0, 0.375, 0.6875, 0.375, 0.5625)
            ],
            [
                (0.3125, 0, 0.375, 0.6875, 0.375, 0.5625)
            ],
            [
                (0.3125, 0, 0.375, 0.6875, 0.375, 0.5625)
            ],
            [
                (0.3125, 0, 0.375, 0.6875, 0.375, 0.5625)
            ],
            [
                (0.3125, 0, 0.375, 0.625, 0.375, 0.6875)
            ],
            [
                (0.3125, 0, 0.375, 0.625, 0.375, 0.6875)
            ],
            [
                (0.3125, 0, 0.375, 0.625, 0.375, 0.6875)
            ],
            [
                (0.3125, 0, 0.375, 0.625, 0.375, 0.6875)
            ],
            [
                (0.3125, 0, 0.3125, 0.6875, 0.375, 0.625)
            ],
            [
                (0.3125, 0, 0.3125, 0.6875, 0.375, 0.625)
            ],
            [
                (0.3125, 0, 0.3125, 0.6875, 0.375, 0.625)
            ],
            [
                (0.3125, 0, 0.3125, 0.6875, 0.375, 0.625)
            ]
        ];
        public override int BlockId => 20869 + (Waterlogged ? 0 : 1) + (Lit ? 0 : 2) + Candles * 4;
        public override int LiquidId => Waterlogged ? 1 : 0;
        public override int LightEmission => 0;
        public override int LightFilter => 0;
        public override (double xa, double ya, double za, double xb, double yb, double zb)[] Collisions => [.. InternalCollisions[BlockId - 20869]];
        public bool Waterlogged = false;
        public bool Lit = false;
        [Range(0, 3)]
        public int Candles = 0;
        public BlockLightGrayCandle()
        {
            
        }
        public override BlockLightGrayCandle Clone()
        {
            return new()
            {
                Waterlogged = Waterlogged,
                Lit = Lit,
                Candles = Candles
            };
        }
        public override Block Break()
        {
            return Waterlogged ? new BlockWater() : new BlockAir();
        }
    }
}
