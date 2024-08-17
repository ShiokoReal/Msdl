using System.ComponentModel.DataAnnotations;
namespace Net.Myzuc.PurpleStainedGlass.Protocol.Blocks
{
    public sealed class BlockFrostedIce : Block
    {
        public override int BlockId => 12539 + Age * 1;
        public override int LiquidId => 0;
        public override int LightEmission => 0;
        public override int LightFilter => 1;
        public override (double xa, double ya, double za, double xb, double yb, double zb)[] Collisions => [
            (0, 0, 0, 1, 1, 1)
        ];
        [Range(0, 3)]
        public int Age = 0;
        public BlockFrostedIce()
        {
            
        }
        public override BlockFrostedIce Clone()
        {
            return new()
            {
                Age = Age
            };
        }
        public override Block Break()
        {
            return new BlockAir();
        }
    }
}
