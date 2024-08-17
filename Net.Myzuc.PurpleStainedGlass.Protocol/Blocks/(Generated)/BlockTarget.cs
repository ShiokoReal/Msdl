using System.ComponentModel.DataAnnotations;
namespace Net.Myzuc.PurpleStainedGlass.Protocol.Blocks
{
    public sealed class BlockTarget : Block
    {
        public override int BlockId => 19381 + Power * 1;
        public override int LiquidId => 0;
        public override int LightEmission => 0;
        public override int LightFilter => 15;
        public override (double xa, double ya, double za, double xb, double yb, double zb)[] Collisions => [
            (0, 0, 0, 1, 1, 1)
        ];
        [Range(0, 15)]
        public int Power = 0;
        public BlockTarget()
        {
            
        }
        public override BlockTarget Clone()
        {
            return new()
            {
                Power = Power
            };
        }
        public override Block Break()
        {
            return new BlockAir();
        }
    }
}
