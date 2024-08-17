using System.ComponentModel.DataAnnotations;
namespace Net.Myzuc.PurpleStainedGlass.Protocol.Blocks
{
    public sealed class BlockPinkPetals : Block
    {
        public enum EnumFacing : int
        {
            North = 0,
            South = 1,
            West = 2,
            East = 3
        }
        public override int BlockId => 24827 + FlowerAmount * 1 + (int)Facing * 4;
        public override int LiquidId => 0;
        public override int LightEmission => 0;
        public override int LightFilter => 0;
        public override (double xa, double ya, double za, double xb, double yb, double zb)[] Collisions => [];
        [Range(0, 3)]
        public int FlowerAmount = 0;
        public EnumFacing Facing = EnumFacing.North;
        public BlockPinkPetals()
        {
            
        }
        public override BlockPinkPetals Clone()
        {
            return new()
            {
                FlowerAmount = FlowerAmount,
                Facing = Facing
            };
        }
        public override Block Break()
        {
            return new BlockAir();
        }
    }
}
