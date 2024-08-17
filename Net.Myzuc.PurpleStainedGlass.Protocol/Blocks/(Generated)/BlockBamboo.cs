using System.ComponentModel.DataAnnotations;
namespace Net.Myzuc.PurpleStainedGlass.Protocol.Blocks
{
    public sealed class BlockBamboo : Block
    {
        public enum EnumLeaves : int
        {
            None = 0,
            Small = 1,
            Large = 2
        }
        private static (double xa, double ya, double za, double xb, double yb, double zb)[][] InternalCollisions = [
            [
                (0.15625, 0, 0.15625, 0.34375, 1, 0.34375)
            ],
            [
                (0.15625, 0, 0.15625, 0.34375, 1, 0.34375)
            ],
            [
                (0.15625, 0, 0.15625, 0.34375, 1, 0.34375)
            ],
            [
                (0.15625, 0, 0.15625, 0.34375, 1, 0.34375)
            ],
            [
                (0.15625, 0, 0.15625, 0.34375, 1, 0.34375)
            ],
            [
                (0.15625, 0, 0.15625, 0.34375, 1, 0.34375)
            ],
            [
                (0.15625, 0, 0.15625, 0.34375, 1, 0.34375)
            ],
            [
                (0.15625, 0, 0.15625, 0.34375, 1, 0.34375)
            ],
            [
                (0.15625, 0, 0.15625, 0.34375, 1, 0.34375)
            ],
            [
                (0.15625, 0, 0.15625, 0.34375, 1, 0.34375)
            ],
            [
                (0.15625, 0, 0.15625, 0.34375, 1, 0.34375)
            ],
            [
                (0.15625, 0, 0.15625, 0.34375, 1, 0.34375)
            ]
        ];
        public override int BlockId => 12945 + Stage * 1 + (int)Leaves * 2 + Age * 6;
        public override int LiquidId => 0;
        public override int LightEmission => 0;
        public override int LightFilter => 0;
        public override (double xa, double ya, double za, double xb, double yb, double zb)[] Collisions => [.. InternalCollisions[BlockId - 12945]];
        [Range(0, 1)]
        public int Stage = 0;
        public EnumLeaves Leaves = EnumLeaves.None;
        [Range(0, 1)]
        public int Age = 0;
        public BlockBamboo()
        {
            
        }
        public override BlockBamboo Clone()
        {
            return new()
            {
                Stage = Stage,
                Leaves = Leaves,
                Age = Age
            };
        }
        public override Block Break()
        {
            return new BlockAir();
        }
    }
}
