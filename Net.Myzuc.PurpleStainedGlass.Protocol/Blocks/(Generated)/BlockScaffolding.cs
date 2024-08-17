using System.ComponentModel.DataAnnotations;
namespace Net.Myzuc.PurpleStainedGlass.Protocol.Blocks
{
    public sealed class BlockScaffolding : Block
    {
        public override int BlockId => 18372 + (Waterlogged ? 0 : 1) + Distance * 2 + (Bottom ? 0 : 16);
        public override int LiquidId => Waterlogged ? 1 : 0;
        public override int LightEmission => 0;
        public override int LightFilter => 0;
        public override (double xa, double ya, double za, double xb, double yb, double zb)[] Collisions => [
            (0, 0, 0, 0.125, 1, 0.125),
            (0, 0, 0.875, 0.125, 1, 1),
            (0.875, 0, 0, 1, 1, 0.125),
            (0.875, 0, 0.875, 1, 1, 1),
            (0, 0.875, 0.125, 1, 1, 0.875),
            (0.125, 0.875, 0, 0.875, 1, 0.125),
            (0.125, 0.875, 0.875, 0.875, 1, 1)
        ];
        public bool Waterlogged = false;
        [Range(0, 7)]
        public int Distance = 0;
        public bool Bottom = false;
        public BlockScaffolding()
        {
            
        }
        public override BlockScaffolding Clone()
        {
            return new()
            {
                Waterlogged = Waterlogged,
                Distance = Distance,
                Bottom = Bottom
            };
        }
        public override Block Break()
        {
            return Waterlogged ? new BlockWater() : new BlockAir();
        }
    }
}
