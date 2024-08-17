using System.ComponentModel.DataAnnotations;
namespace Net.Myzuc.PurpleStainedGlass.Protocol.Blocks
{
    public sealed class BlockWhiteBanner : Block
    {
        public override int BlockId => 10759 + Rotation * 1;
        public override int LiquidId => 0;
        public override int LightEmission => 0;
        public override int LightFilter => 0;
        public override (double xa, double ya, double za, double xb, double yb, double zb)[] Collisions => [];
        [Range(0, 15)]
        public int Rotation = 0;
        public BlockWhiteBanner()
        {
            
        }
        public override BlockWhiteBanner Clone()
        {
            return new()
            {
                Rotation = Rotation
            };
        }
        public override Block Break()
        {
            return new BlockAir();
        }
    }
}
