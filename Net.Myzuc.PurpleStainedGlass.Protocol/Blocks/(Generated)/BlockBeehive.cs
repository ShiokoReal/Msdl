using System.ComponentModel.DataAnnotations;
namespace Net.Myzuc.PurpleStainedGlass.Protocol.Blocks
{
    public sealed class BlockBeehive : Block
    {
        public enum EnumFacing : int
        {
            North = 0,
            South = 1,
            West = 2,
            East = 3
        }
        public override int BlockId => 19421 + HoneyLevel * 1 + (int)Facing * 6;
        public override int LiquidId => 0;
        public override int LightEmission => 0;
        public override int LightFilter => 15;
        public override (double xa, double ya, double za, double xb, double yb, double zb)[] Collisions => [
            (0, 0, 0, 1, 1, 1)
        ];
        [Range(0, 5)]
        public int HoneyLevel = 0;
        public EnumFacing Facing = EnumFacing.North;
        public BlockBeehive()
        {
            
        }
        public override BlockBeehive Clone()
        {
            return new()
            {
                HoneyLevel = HoneyLevel,
                Facing = Facing
            };
        }
        public override Block Break()
        {
            return new BlockAir();
        }
    }
}
