using System.ComponentModel.DataAnnotations;
namespace Net.Myzuc.PurpleStainedGlass.Protocol.Blocks
{
    public sealed class BlockChorusFlower : Block
    {
        public override int BlockId => 12404 + Age * 1;
        public override int LiquidId => 0;
        public override int LightEmission => 0;
        public override int LightFilter => 1;
        public override (double xa, double ya, double za, double xb, double yb, double zb)[] Collisions => [
            (0, 0, 0, 1, 1, 1)
        ];
        [Range(0, 5)]
        public int Age = 0;
        public BlockChorusFlower()
        {
            
        }
        public override BlockChorusFlower Clone()
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
