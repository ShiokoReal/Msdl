using System.ComponentModel.DataAnnotations;
namespace Net.Myzuc.PurpleStainedGlass.Protocol.Blocks
{
    public sealed class BlockPitcherCrop : Block
    {
        public enum EnumHalf : int
        {
            Upper = 0,
            Lower = 1
        }
        private static (double xa, double ya, double za, double xb, double yb, double zb)[][] InternalCollisions = [
            [
                (0.3125, -0.0625, 0.3125, 0.6875, 0.1875, 0.6875)
            ],
            [
                (0.3125, -0.0625, 0.3125, 0.6875, 0.1875, 0.6875)
            ],
            [],
            [
                (0.1875, -0.0625, 0.1875, 0.8125, 0.3125, 0.8125)
            ],
            [],
            [
                (0.1875, -0.0625, 0.1875, 0.8125, 0.3125, 0.8125)
            ],
            [],
            [
                (0.1875, -0.0625, 0.1875, 0.8125, 0.3125, 0.8125)
            ],
            [],
            [
                (0.1875, -0.0625, 0.1875, 0.8125, 0.3125, 0.8125)
            ]
        ];
        public override int BlockId => 12497 + (int)Half * 1 + Age * 2;
        public override int LiquidId => 0;
        public override int LightEmission => 0;
        public override int LightFilter => 0;
        public override (double xa, double ya, double za, double xb, double yb, double zb)[] Collisions => [.. InternalCollisions[BlockId - 12497]];
        public EnumHalf Half = EnumHalf.Upper;
        [Range(0, 4)]
        public int Age = 0;
        public BlockPitcherCrop()
        {
            
        }
        public override BlockPitcherCrop Clone()
        {
            return new()
            {
                Half = Half,
                Age = Age
            };
        }
        public override Block Break()
        {
            return new BlockAir();
        }
    }
}
