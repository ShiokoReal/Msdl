using System.ComponentModel.DataAnnotations;
namespace Net.Myzuc.PurpleStainedGlass.Protocol.Blocks
{
    public sealed class BlockSnifferEgg : Block
    {
        public override int BlockId => 12800 + Hatch * 1;
        public override int LiquidId => 0;
        public override int LightEmission => 0;
        public override int LightFilter => 0;
        public override (double xa, double ya, double za, double xb, double yb, double zb)[] Collisions => [
            (0.0625, 0, 0.125, 0.9375, 1, 0.875)
        ];
        [Range(0, 2)]
        public int Hatch = 0;
        public BlockSnifferEgg()
        {
            
        }
        public override BlockSnifferEgg Clone()
        {
            return new()
            {
                Hatch = Hatch
            };
        }
        public override Block Break()
        {
            return new BlockAir();
        }
    }
}
