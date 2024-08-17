using System.ComponentModel.DataAnnotations;
namespace Net.Myzuc.PurpleStainedGlass.Protocol.Blocks
{
    public sealed class BlockSuspiciousSand : Block
    {
        public override int BlockId => 113 + Dusted * 1;
        public override int LiquidId => 0;
        public override int LightEmission => 0;
        public override int LightFilter => 15;
        public override (double xa, double ya, double za, double xb, double yb, double zb)[] Collisions => [
            (0, 0, 0, 1, 1, 1)
        ];
        [Range(0, 3)]
        public int Dusted = 0;
        public BlockSuspiciousSand()
        {
            
        }
        public override BlockSuspiciousSand Clone()
        {
            return new()
            {
                Dusted = Dusted
            };
        }
        public override Block Break()
        {
            return new BlockAir();
        }
    }
}
