using System.ComponentModel.DataAnnotations;
namespace Net.Myzuc.PurpleStainedGlass.Protocol.Blocks
{
    public sealed class BlockFarmland : Block
    {
        public override int BlockId => 4286 + Moisture * 1;
        public override int LiquidId => 0;
        public override int LightEmission => 0;
        public override int LightFilter => 0;
        public override (double xa, double ya, double za, double xb, double yb, double zb)[] Collisions => [
            (0, 0, 0, 1, 0.9375, 1)
        ];
        [Range(0, 7)]
        public int Moisture = 0;
        public BlockFarmland()
        {
            
        }
        public override BlockFarmland Clone()
        {
            return new()
            {
                Moisture = Moisture
            };
        }
        public override Block Break()
        {
            return new BlockAir();
        }
    }
}
