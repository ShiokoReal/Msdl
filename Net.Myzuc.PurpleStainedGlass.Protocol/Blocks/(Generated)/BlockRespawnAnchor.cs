using System.ComponentModel.DataAnnotations;
namespace Net.Myzuc.PurpleStainedGlass.Protocol.Blocks
{
    public sealed class BlockRespawnAnchor : Block
    {
        public override int BlockId => 19450 + Charges * 1;
        public override int LiquidId => 0;
        public override int LightEmission => 0;
        public override int LightFilter => 15;
        public override (double xa, double ya, double za, double xb, double yb, double zb)[] Collisions => [
            (0, 0, 0, 1, 1, 1)
        ];
        [Range(0, 4)]
        public int Charges = 0;
        public BlockRespawnAnchor()
        {
            
        }
        public override BlockRespawnAnchor Clone()
        {
            return new()
            {
                Charges = Charges
            };
        }
        public override Block Break()
        {
            return new BlockAir();
        }
    }
}
