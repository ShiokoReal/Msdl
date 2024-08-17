using System.ComponentModel.DataAnnotations;
namespace Net.Myzuc.PurpleStainedGlass.Protocol.Blocks
{
    public sealed class BlockSeaPickle : Block
    {
        private static (double xa, double ya, double za, double xb, double yb, double zb)[][] InternalCollisions = [
            [
                (0.375, 0, 0.375, 0.625, 0.375, 0.625)
            ],
            [
                (0.375, 0, 0.375, 0.625, 0.375, 0.625)
            ],
            [
                (0.1875, 0, 0.1875, 0.8125, 0.375, 0.8125)
            ],
            [
                (0.1875, 0, 0.1875, 0.8125, 0.375, 0.8125)
            ],
            [
                (0.125, 0, 0.125, 0.875, 0.375, 0.875)
            ],
            [
                (0.125, 0, 0.125, 0.875, 0.375, 0.875)
            ],
            [
                (0.125, 0, 0.125, 0.875, 0.4375, 0.875)
            ],
            [
                (0.125, 0, 0.125, 0.875, 0.4375, 0.875)
            ]
        ];
        public override int BlockId => 12933 + (Waterlogged ? 0 : 1) + Pickles * 2;
        public override int LiquidId => Waterlogged ? 1 : 0;
        public override int LightEmission => 6;
        public override int LightFilter => 1;
        public override (double xa, double ya, double za, double xb, double yb, double zb)[] Collisions => [.. InternalCollisions[BlockId - 12933]];
        public bool Waterlogged = false;
        [Range(0, 3)]
        public int Pickles = 0;
        public BlockSeaPickle()
        {
            
        }
        public override BlockSeaPickle Clone()
        {
            return new()
            {
                Waterlogged = Waterlogged,
                Pickles = Pickles
            };
        }
        public override Block Break()
        {
            return Waterlogged ? new BlockWater() : new BlockAir();
        }
    }
}
