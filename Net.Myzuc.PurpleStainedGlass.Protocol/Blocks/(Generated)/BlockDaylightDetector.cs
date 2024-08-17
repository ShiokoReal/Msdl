using System.ComponentModel.DataAnnotations;
namespace Net.Myzuc.PurpleStainedGlass.Protocol.Blocks
{
    public sealed class BlockDaylightDetector : Block
    {
        public override int BlockId => 9191 + Power * 1 + (Inverted ? 0 : 16);
        public override int LiquidId => 0;
        public override int LightEmission => 0;
        public override int LightFilter => 0;
        public override (double xa, double ya, double za, double xb, double yb, double zb)[] Collisions => [
            (0, 0, 0, 1, 0.375, 1)
        ];
        [Range(0, 15)]
        public int Power = 0;
        public bool Inverted = false;
        public BlockDaylightDetector()
        {
            
        }
        public override BlockDaylightDetector Clone()
        {
            return new()
            {
                Power = Power,
                Inverted = Inverted
            };
        }
        public override Block Break()
        {
            return new BlockAir();
        }
    }
}
