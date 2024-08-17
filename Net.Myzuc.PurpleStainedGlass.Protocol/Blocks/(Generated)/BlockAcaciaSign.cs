using System.ComponentModel.DataAnnotations;
namespace Net.Myzuc.PurpleStainedGlass.Protocol.Blocks
{
    public sealed class BlockAcaciaSign : Block
    {
        public override int BlockId => 4398 + (Waterlogged ? 0 : 1) + Rotation * 2;
        public override int LiquidId => Waterlogged ? 1 : 0;
        public override int LightEmission => 0;
        public override int LightFilter => 0;
        public override (double xa, double ya, double za, double xb, double yb, double zb)[] Collisions => [];
        public bool Waterlogged = false;
        [Range(0, 15)]
        public int Rotation = 0;
        public BlockAcaciaSign()
        {
            
        }
        public override BlockAcaciaSign Clone()
        {
            return new()
            {
                Waterlogged = Waterlogged,
                Rotation = Rotation
            };
        }
        public override Block Break()
        {
            return Waterlogged ? new BlockWater() : new BlockAir();
        }
    }
}
