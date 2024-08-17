using System.ComponentModel.DataAnnotations;
namespace Net.Myzuc.PurpleStainedGlass.Protocol.Blocks
{
    public sealed class BlockCaveVines : Block
    {
        public override int BlockId => 24769 + (Berries ? 0 : 1) + Age * 2;
        public override int LiquidId => 0;
        public override int LightEmission => 0;
        public override int LightFilter => 0;
        public override (double xa, double ya, double za, double xb, double yb, double zb)[] Collisions => [];
        public bool Berries = false;
        [Range(0, 25)]
        public int Age = 0;
        public BlockCaveVines()
        {
            
        }
        public override BlockCaveVines Clone()
        {
            return new()
            {
                Berries = Berries,
                Age = Age
            };
        }
        public override Block Break()
        {
            return new BlockAir();
        }
    }
}
