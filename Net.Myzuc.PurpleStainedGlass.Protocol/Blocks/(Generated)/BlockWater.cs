using System.ComponentModel.DataAnnotations;
namespace Net.Myzuc.PurpleStainedGlass.Protocol.Blocks
{
    public sealed class BlockWater : Block
    {
        public override int BlockId => 80 + Level * 1;
        public override int LiquidId => 1;
        public override int LightEmission => 0;
        public override int LightFilter => 1;
        public override (double xa, double ya, double za, double xb, double yb, double zb)[] Collisions => [];
        [Range(0, 15)]
        public int Level = 0;
        public BlockWater()
        {
            
        }
        public override BlockWater Clone()
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
