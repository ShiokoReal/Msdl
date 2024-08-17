using System.ComponentModel.DataAnnotations;
namespace Net.Myzuc.PurpleStainedGlass.Protocol.Blocks
{
    public sealed class BlockLight : Block
    {
        public override int BlockId => 10367 + (Waterlogged ? 0 : 1) + Level * 2;
        public override int LiquidId => Waterlogged ? 1 : 0;
        public override int LightEmission => 15;
        public override int LightFilter => 0;
        public override (double xa, double ya, double za, double xb, double yb, double zb)[] Collisions => [];
        public bool Waterlogged = false;
        [Range(0, 15)]
        public int Level = 0;
        public BlockLight()
        {
            
        }
        public override BlockLight Clone()
        {
            return new()
            {
                Waterlogged = Waterlogged,
                Level = Level
            };
        }
        public override Block Break()
        {
            return Waterlogged ? new BlockWater() : new BlockAir();
        }
    }
}
