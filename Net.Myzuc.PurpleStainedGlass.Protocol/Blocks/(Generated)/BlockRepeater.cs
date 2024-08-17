using System.ComponentModel.DataAnnotations;
namespace Net.Myzuc.PurpleStainedGlass.Protocol.Blocks
{
    public sealed class BlockRepeater : Block
    {
        public enum EnumFacing : int
        {
            North = 0,
            South = 1,
            West = 2,
            East = 3
        }
        public override int BlockId => 5881 + (Powered ? 0 : 1) + (Locked ? 0 : 2) + (int)Facing * 4 + Delay * 16;
        public override int LiquidId => 0;
        public override int LightEmission => 0;
        public override int LightFilter => 0;
        public override (double xa, double ya, double za, double xb, double yb, double zb)[] Collisions => [
            (0, 0, 0, 1, 0.125, 1)
        ];
        public bool Powered = false;
        public bool Locked = false;
        public EnumFacing Facing = EnumFacing.North;
        [Range(0, 3)]
        public int Delay = 0;
        public BlockRepeater()
        {
            
        }
        public override BlockRepeater Clone()
        {
            return new()
            {
                Powered = Powered,
                Locked = Locked,
                Facing = Facing,
                Delay = Delay
            };
        }
        public override Block Break()
        {
            return new BlockAir();
        }
    }
}
