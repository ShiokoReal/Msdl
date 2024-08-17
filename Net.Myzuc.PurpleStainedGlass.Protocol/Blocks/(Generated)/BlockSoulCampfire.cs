namespace Net.Myzuc.PurpleStainedGlass.Protocol.Blocks
{
    public sealed class BlockSoulCampfire : Block
    {
        public enum EnumFacing : int
        {
            North = 0,
            South = 1,
            West = 2,
            East = 3
        }
        public override int BlockId => 18543 + (Waterlogged ? 0 : 1) + (SignalFire ? 0 : 2) + (Lit ? 0 : 4) + (int)Facing * 8;
        public override int LiquidId => Waterlogged ? 1 : 0;
        public override int LightEmission => 10;
        public override int LightFilter => 0;
        public override (double xa, double ya, double za, double xb, double yb, double zb)[] Collisions => [
            (0, 0, 0, 1, 0.4375, 1)
        ];
        public bool Waterlogged = false;
        public bool SignalFire = false;
        public bool Lit = false;
        public EnumFacing Facing = EnumFacing.North;
        public BlockSoulCampfire()
        {
            
        }
        public override BlockSoulCampfire Clone()
        {
            return new()
            {
                Waterlogged = Waterlogged,
                SignalFire = SignalFire,
                Lit = Lit,
                Facing = Facing
            };
        }
        public override Block Break()
        {
            return Waterlogged ? new BlockWater() : new BlockAir();
        }
    }
}
