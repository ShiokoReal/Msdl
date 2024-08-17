namespace Net.Myzuc.PurpleStainedGlass.Protocol.Blocks
{
    public sealed class BlockNetherPortal : Block
    {
        public enum EnumAxis : int
        {
            X = 0,
            Z = 1
        }
        public override int BlockId => 5864 + (int)Axis * 1;
        public override int LiquidId => 0;
        public override int LightEmission => 11;
        public override int LightFilter => 0;
        public override (double xa, double ya, double za, double xb, double yb, double zb)[] Collisions => [];
        public EnumAxis Axis = EnumAxis.X;
        public BlockNetherPortal()
        {
            
        }
        public override BlockNetherPortal Clone()
        {
            return new()
            {
                Axis = Axis
            };
        }
        public override Block Break()
        {
            return new BlockAir();
        }
    }
}
