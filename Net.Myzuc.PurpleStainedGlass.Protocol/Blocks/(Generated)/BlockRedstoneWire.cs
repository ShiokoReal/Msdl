using System.ComponentModel.DataAnnotations;
namespace Net.Myzuc.PurpleStainedGlass.Protocol.Blocks
{
    public sealed class BlockRedstoneWire : Block
    {
        public enum EnumWest : int
        {
            Up = 0,
            Side = 1,
            None = 2
        }
        public enum EnumSouth : int
        {
            Up = 0,
            Side = 1,
            None = 2
        }
        public enum EnumNorth : int
        {
            Up = 0,
            Side = 1,
            None = 2
        }
        public enum EnumEast : int
        {
            Up = 0,
            Side = 1,
            None = 2
        }
        public override int BlockId => 2978 + (int)West * 1 + (int)South * 3 + Power * 9 + (int)North * 144 + (int)East * 432;
        public override int LiquidId => 0;
        public override int LightEmission => 0;
        public override int LightFilter => 0;
        public override (double xa, double ya, double za, double xb, double yb, double zb)[] Collisions => [];
        public EnumWest West = EnumWest.Up;
        public EnumSouth South = EnumSouth.Up;
        [Range(0, 15)]
        public int Power = 0;
        public EnumNorth North = EnumNorth.Up;
        public EnumEast East = EnumEast.Up;
        public BlockRedstoneWire()
        {
            
        }
        public override BlockRedstoneWire Clone()
        {
            return new()
            {
                West = West,
                South = South,
                Power = Power,
                North = North,
                East = East
            };
        }
        public override Block Break()
        {
            return new BlockAir();
        }
    }
}
