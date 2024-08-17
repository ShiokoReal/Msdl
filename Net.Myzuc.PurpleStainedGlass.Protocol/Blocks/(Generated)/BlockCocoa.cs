using System.ComponentModel.DataAnnotations;
namespace Net.Myzuc.PurpleStainedGlass.Protocol.Blocks
{
    public sealed class BlockCocoa : Block
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
                (0.375, 0.4375, 0.0625, 0.625, 0.75, 0.3125)
            ],
            [
                (0.375, 0.4375, 0.6875, 0.625, 0.75, 0.9375)
            ],
            [
                (0.0625, 0.4375, 0.375, 0.3125, 0.75, 0.625)
            ],
            [
                (0.6875, 0.4375, 0.375, 0.9375, 0.75, 0.625)
            ],
            [
                (0.3125, 0.3125, 0.0625, 0.6875, 0.75, 0.4375)
            ],
            [
                (0.3125, 0.3125, 0.5625, 0.6875, 0.75, 0.9375)
            ],
            [
                (0.0625, 0.3125, 0.3125, 0.4375, 0.75, 0.6875)
            ],
            [
                (0.5625, 0.3125, 0.3125, 0.9375, 0.75, 0.6875)
            ],
            [
                (0.25, 0.1875, 0.0625, 0.75, 0.75, 0.5625)
            ],
            [
                (0.25, 0.1875, 0.4375, 0.75, 0.75, 0.9375)
            ],
            [
                (0.0625, 0.1875, 0.25, 0.5625, 0.75, 0.75)
            ],
            [
                (0.4375, 0.1875, 0.25, 0.9375, 0.75, 0.75)
            ]
        ];
        public override int BlockId => 7419 + (int)Facing * 1 + Age * 4;
        public override int LiquidId => 0;
        public override int LightEmission => 0;
        public override int LightFilter => 0;
        public override (double xa, double ya, double za, double xb, double yb, double zb)[] Collisions => [.. InternalCollisions[BlockId - 7419]];
        public EnumFacing Facing = EnumFacing.North;
        [Range(0, 2)]
        public int Age = 0;
        public BlockCocoa()
        {
            
        }
        public override BlockCocoa Clone()
        {
            return new()
            {
                Facing = Facing,
                Age = Age
            };
        }
        public override Block Break()
        {
            return new BlockAir();
        }
    }
}
