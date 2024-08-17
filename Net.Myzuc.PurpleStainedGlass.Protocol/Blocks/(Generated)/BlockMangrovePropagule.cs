using System.ComponentModel.DataAnnotations;
namespace Net.Myzuc.PurpleStainedGlass.Protocol.Blocks
{
    public sealed class BlockMangrovePropagule : Block
    {
        public override int BlockId => 39 + (Waterlogged ? 0 : 1) + Stage * 2 + (Hanging ? 0 : 4) + Age * 8;
        public override int LiquidId => Waterlogged ? 1 : 0;
        public override int LightEmission => 0;
        public override int LightFilter => 0;
        public override (double xa, double ya, double za, double xb, double yb, double zb)[] Collisions => [];
        public bool Waterlogged = false;
        [Range(0, 1)]
        public int Stage = 0;
        public bool Hanging = false;
        [Range(0, 4)]
        public int Age = 0;
        public BlockMangrovePropagule()
        {
            
        }
        public override BlockMangrovePropagule Clone()
        {
            return new()
            {
                Waterlogged = Waterlogged,
                Stage = Stage,
                Hanging = Hanging,
                Age = Age
            };
        }
        public override Block Break()
        {
            return Waterlogged ? new BlockWater() : new BlockAir();
        }
    }
}
