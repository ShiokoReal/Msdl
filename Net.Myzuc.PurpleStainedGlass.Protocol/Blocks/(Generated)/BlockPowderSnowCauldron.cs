using System.ComponentModel.DataAnnotations;
namespace Net.Myzuc.PurpleStainedGlass.Protocol.Blocks
{
    public sealed class BlockPowderSnowCauldron : Block
    {
        public override int BlockId => 7403 + Level * 1;
        public override int LiquidId => 0;
        public override int LightEmission => 0;
        public override int LightFilter => 0;
        public override (double xa, double ya, double za, double xb, double yb, double zb)[] Collisions => [
            (0, 0, 0, 0.125, 1, 0.25),
            (0, 0, 0.75, 0.125, 1, 1),
            (0.125, 0, 0, 0.25, 1, 0.125),
            (0.125, 0, 0.875, 0.25, 1, 1),
            (0.75, 0, 0, 1, 1, 0.125),
            (0.75, 0, 0.875, 1, 1, 1),
            (0.875, 0, 0.125, 1, 1, 0.25),
            (0.875, 0, 0.75, 1, 1, 0.875),
            (0, 0.1875, 0.25, 1, 0.25, 0.75),
            (0.125, 0.1875, 0.125, 0.875, 0.25, 0.25),
            (0.125, 0.1875, 0.75, 0.875, 0.25, 0.875),
            (0.25, 0.1875, 0, 0.75, 1, 0.125),
            (0.25, 0.1875, 0.875, 0.75, 1, 1),
            (0, 0.25, 0.25, 0.125, 1, 0.75),
            (0.875, 0.25, 0.25, 1, 1, 0.75)
        ];
        [Range(0, 2)]
        public int Level = 0;
        public BlockPowderSnowCauldron()
        {
            
        }
        public override BlockPowderSnowCauldron Clone()
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
