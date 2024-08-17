using System.ComponentModel.DataAnnotations;
namespace Net.Myzuc.PurpleStainedGlass.Protocol.Blocks
{
    public sealed class BlockPumpkinStem : Block
    {
        public override int BlockId => 6821 + Age * 1;
        public override int LiquidId => 0;
        public override int LightEmission => 0;
        public override int LightFilter => 0;
        public override (double xa, double ya, double za, double xb, double yb, double zb)[] Collisions => [];
        [Range(0, 7)]
        public int Age = 0;
        public BlockPumpkinStem()
        {
            
        }
        public override BlockPumpkinStem Clone()
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
