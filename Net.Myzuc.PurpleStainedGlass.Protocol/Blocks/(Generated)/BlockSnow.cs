using System.ComponentModel.DataAnnotations;
namespace Net.Myzuc.PurpleStainedGlass.Protocol.Blocks
{
    public sealed class BlockSnow : Block
    {
        private static (double xa, double ya, double za, double xb, double yb, double zb)[][] InternalCollisions = [
            [],
            [
                (0, 0, 0, 1, 0.125, 1)
            ],
            [
                (0, 0, 0, 1, 0.25, 1)
            ],
            [
                (0, 0, 0, 1, 0.375, 1)
            ],
            [
                (0, 0, 0, 1, 0.5, 1)
            ],
            [
                (0, 0, 0, 1, 0.625, 1)
            ],
            [
                (0, 0, 0, 1, 0.75, 1)
            ],
            [
                (0, 0, 0, 1, 0.875, 1)
            ]
        ];
        public override int BlockId => 5772 + Layers * 1;
        public override int LiquidId => 0;
        public override int LightEmission => 0;
        public override int LightFilter => 0;
        public override (double xa, double ya, double za, double xb, double yb, double zb)[] Collisions => [.. InternalCollisions[BlockId - 5772]];
        [Range(0, 7)]
        public int Layers = 0;
        public BlockSnow()
        {
            
        }
        public override BlockSnow Clone()
        {
            return new()
            {
                Layers = Layers
            };
        }
        public override Block Break()
        {
            return new BlockAir();
        }
    }
}
