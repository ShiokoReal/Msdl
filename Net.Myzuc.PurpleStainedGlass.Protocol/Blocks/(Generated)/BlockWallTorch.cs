namespace Net.Myzuc.PurpleStainedGlass.Protocol.Blocks
{
    public sealed class BlockWallTorch : Block
    {
        public enum EnumFacing : int
        {
            North = 0,
            South = 1,
            West = 2,
            East = 3
        }
        public override int BlockId => 2356 + (int)Facing * 1;
        public override int LiquidId => 0;
        public override int LightEmission => 14;
        public override int LightFilter => 0;
        public override (double xa, double ya, double za, double xb, double yb, double zb)[] Collisions => [];
        public EnumFacing Facing = EnumFacing.North;
        public BlockWallTorch()
        {
            
        }
        public override BlockWallTorch Clone()
        {
            return new()
            {
                Facing = Facing
            };
        }
        public override Block Break()
        {
            return new BlockAir();
        }
    }
}
