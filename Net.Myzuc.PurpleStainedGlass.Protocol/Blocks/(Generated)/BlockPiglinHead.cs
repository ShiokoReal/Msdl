using System.ComponentModel.DataAnnotations;
namespace Net.Myzuc.PurpleStainedGlass.Protocol.Blocks
{
    public sealed class BlockPiglinHead : Block
    {
        public override int BlockId => 9067 + Rotation * 1 + (Powered ? 0 : 16);
        public override int LiquidId => 0;
        public override int LightEmission => 0;
        public override int LightFilter => 0;
        public override (double xa, double ya, double za, double xb, double yb, double zb)[] Collisions => [
            (0.1875, 0, 0.1875, 0.8125, 0.5, 0.8125)
        ];
        [Range(0, 15)]
        public int Rotation = 0;
        public bool Powered = false;
        public BlockPiglinHead()
        {
            
        }
        public override BlockPiglinHead Clone()
        {
            return new()
            {
                Rotation = Rotation,
                Powered = Powered
            };
        }
        public override Block Break()
        {
            return new BlockAir();
        }
    }
}
