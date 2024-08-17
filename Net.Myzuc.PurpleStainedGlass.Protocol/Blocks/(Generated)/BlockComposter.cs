using System.ComponentModel.DataAnnotations;
namespace Net.Myzuc.PurpleStainedGlass.Protocol.Blocks
{
    public sealed class BlockComposter : Block
    {
        public override int BlockId => 19372 + Level * 1;
        public override int LiquidId => 0;
        public override int LightEmission => 0;
        public override int LightFilter => 0;
        public override (double xa, double ya, double za, double xb, double yb, double zb)[] Collisions => [
            (0, 0, 0, 1, 0.125, 1),
            (0, 0.125, 0, 0.125, 1, 1),
            (0.125, 0.125, 0, 1, 1, 0.125),
            (0.125, 0.125, 0.875, 1, 1, 1),
            (0.875, 0.125, 0.125, 1, 1, 0.875)
        ];
        [Range(0, 8)]
        public int Level = 0;
        public BlockComposter()
        {
            
        }
        public override BlockComposter Clone()
        {
            return new()
            {
                Level = Level
            };
        }
        public override Block Break()
        {
            return new BlockAir();
        }
    }
}
