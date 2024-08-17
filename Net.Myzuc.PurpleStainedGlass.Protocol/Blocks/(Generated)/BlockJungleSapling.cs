using System.ComponentModel.DataAnnotations;
namespace Net.Myzuc.PurpleStainedGlass.Protocol.Blocks
{
    public sealed class BlockJungleSapling : Block
    {
        public override int BlockId => 31 + Stage * 1;
        public override int LiquidId => 0;
        public override int LightEmission => 0;
        public override int LightFilter => 0;
        public override (double xa, double ya, double za, double xb, double yb, double zb)[] Collisions => [];
        [Range(0, 1)]
        public int Stage = 0;
        public BlockJungleSapling()
        {
            
        }
        public override BlockJungleSapling Clone()
        {
            return new()
            {
                Stage = Stage
            };
        }
        public override Block Break()
        {
            return new BlockAir();
        }
    }
}
