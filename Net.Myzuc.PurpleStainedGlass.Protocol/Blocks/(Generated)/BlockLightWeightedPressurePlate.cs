using System.ComponentModel.DataAnnotations;
namespace Net.Myzuc.PurpleStainedGlass.Protocol.Blocks
{
    public sealed class BlockLightWeightedPressurePlate : Block
    {
        public override int BlockId => 9143 + Power * 1;
        public override int LiquidId => 0;
        public override int LightEmission => 0;
        public override int LightFilter => 0;
        public override (double xa, double ya, double za, double xb, double yb, double zb)[] Collisions => [];
        [Range(0, 15)]
        public int Power = 0;
        public BlockLightWeightedPressurePlate()
        {
            
        }
        public override BlockLightWeightedPressurePlate Clone()
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
