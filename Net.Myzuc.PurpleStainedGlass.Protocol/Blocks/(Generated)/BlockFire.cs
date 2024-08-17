using System.ComponentModel.DataAnnotations;
namespace Net.Myzuc.PurpleStainedGlass.Protocol.Blocks
{
    public sealed class BlockFire : Block
    {
        public override int BlockId => 2360 + (West ? 0 : 1) + (Up ? 0 : 2) + (South ? 0 : 4) + (North ? 0 : 8) + (East ? 0 : 16) + Age * 32;
        public override int LiquidId => 0;
        public override int LightEmission => 15;
        public override int LightFilter => 0;
        public override (double xa, double ya, double za, double xb, double yb, double zb)[] Collisions => [];
        public bool West = false;
        public bool Up = false;
        public bool South = false;
        public bool North = false;
        public bool East = false;
        [Range(0, 15)]
        public int Age = 0;
        public BlockFire()
        {
            
        }
        public override BlockFire Clone()
        {
            return new()
            {
                West = West,
                Up = Up,
                South = South,
                North = North,
                East = East,
                Age = Age
            };
        }
        public override Block Break()
        {
            return new BlockAir();
        }
    }
}
