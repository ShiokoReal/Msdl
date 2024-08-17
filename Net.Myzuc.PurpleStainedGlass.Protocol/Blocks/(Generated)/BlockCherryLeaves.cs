using System.ComponentModel.DataAnnotations;
namespace Net.Myzuc.PurpleStainedGlass.Protocol.Blocks
{
    public sealed class BlockCherryLeaves : Block
    {
        public override int BlockId => 377 + (Waterlogged ? 0 : 1) + (Persistent ? 0 : 2) + Distance * 4;
        public override int LiquidId => Waterlogged ? 1 : 0;
        public override int LightEmission => 0;
        public override int LightFilter => 1;
        public override (double xa, double ya, double za, double xb, double yb, double zb)[] Collisions => [
            (0, 0, 0, 1, 1, 1)
        ];
        public bool Waterlogged = false;
        public bool Persistent = false;
        [Range(0, 6)]
        public int Distance = 0;
        public BlockCherryLeaves()
        {
            
        }
        public override BlockCherryLeaves Clone()
        {
            return new()
            {
                Waterlogged = Waterlogged,
                Persistent = Persistent,
                Distance = Distance
            };
        }
        public override Block Break()
        {
            return Waterlogged ? new BlockWater() : new BlockAir();
        }
    }
}
