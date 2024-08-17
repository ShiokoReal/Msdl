using System.ComponentModel.DataAnnotations;
namespace Net.Myzuc.PurpleStainedGlass.Protocol.Blocks
{
    public sealed class BlockCake : Block
    {
        private static (double xa, double ya, double za, double xb, double yb, double zb)[][] InternalCollisions = [
            [
                (0.0625, 0, 0.0625, 0.9375, 0.5, 0.9375)
            ],
            [
                (0.1875, 0, 0.0625, 0.9375, 0.5, 0.9375)
            ],
            [
                (0.3125, 0, 0.0625, 0.9375, 0.5, 0.9375)
            ],
            [
                (0.4375, 0, 0.0625, 0.9375, 0.5, 0.9375)
            ],
            [
                (0.5625, 0, 0.0625, 0.9375, 0.5, 0.9375)
            ],
            [
                (0.6875, 0, 0.0625, 0.9375, 0.5, 0.9375)
            ],
            [
                (0.8125, 0, 0.0625, 0.9375, 0.5, 0.9375)
            ]
        ];
        public override int BlockId => 5874 + Bites * 1;
        public override int LiquidId => 0;
        public override int LightEmission => 0;
        public override int LightFilter => 0;
        public override (double xa, double ya, double za, double xb, double yb, double zb)[] Collisions => [.. InternalCollisions[BlockId - 5874]];
        [Range(0, 6)]
        public int Bites = 0;
        public BlockCake()
        {
            
        }
        public override BlockCake Clone()
        {
            return new()
            {
                Bites = Bites
            };
        }
        public override Block Break()
        {
            return new BlockAir();
        }
    }
}
