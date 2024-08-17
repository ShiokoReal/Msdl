using System.ComponentModel.DataAnnotations;
namespace Net.Myzuc.PurpleStainedGlass.Protocol.Blocks
{
    public sealed class BlockTurtleEgg : Block
    {
        private static (double xa, double ya, double za, double xb, double yb, double zb)[][] InternalCollisions = [
            [
                (0.1875, 0, 0.1875, 0.75, 0.4375, 0.75)
            ],
            [
                (0.1875, 0, 0.1875, 0.75, 0.4375, 0.75)
            ],
            [
                (0.1875, 0, 0.1875, 0.75, 0.4375, 0.75)
            ],
            [
                (0.0625, 0, 0.0625, 0.9375, 0.4375, 0.9375)
            ],
            [
                (0.0625, 0, 0.0625, 0.9375, 0.4375, 0.9375)
            ],
            [
                (0.0625, 0, 0.0625, 0.9375, 0.4375, 0.9375)
            ],
            [
                (0.0625, 0, 0.0625, 0.9375, 0.4375, 0.9375)
            ],
            [
                (0.0625, 0, 0.0625, 0.9375, 0.4375, 0.9375)
            ],
            [
                (0.0625, 0, 0.0625, 0.9375, 0.4375, 0.9375)
            ],
            [
                (0.0625, 0, 0.0625, 0.9375, 0.4375, 0.9375)
            ],
            [
                (0.0625, 0, 0.0625, 0.9375, 0.4375, 0.9375)
            ],
            [
                (0.0625, 0, 0.0625, 0.9375, 0.4375, 0.9375)
            ]
        ];
        public override int BlockId => 12788 + Hatch * 1 + Eggs * 3;
        public override int LiquidId => 0;
        public override int LightEmission => 0;
        public override int LightFilter => 0;
        public override (double xa, double ya, double za, double xb, double yb, double zb)[] Collisions => [.. InternalCollisions[BlockId - 12788]];
        [Range(0, 2)]
        public int Hatch = 0;
        [Range(0, 3)]
        public int Eggs = 0;
        public BlockTurtleEgg()
        {
            
        }
        public override BlockTurtleEgg Clone()
        {
            return new()
            {
                Hatch = Hatch,
                Eggs = Eggs
            };
        }
        public override Block Break()
        {
            return new BlockAir();
        }
    }
}
