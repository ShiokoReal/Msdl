namespace Net.Myzuc.PurpleStainedGlass.Protocol.Blocks
{
    public sealed class BlockDeepslate : Block
    {
        public enum EnumAxis : int
        {
            X = 0,
            Y = 1,
            Z = 2
        }
        public override int BlockId => 24904 + (int)Axis * 1;
        public override int LiquidId => 0;
        public override int LightEmission => 0;
        public override int LightFilter => 15;
        public override (double xa, double ya, double za, double xb, double yb, double zb)[] Collisions => [
            (0, 0, 0, 1, 1, 1)
        ];
        public EnumAxis Axis = EnumAxis.X;
        public BlockDeepslate()
        {
            
        }
        public override BlockDeepslate Clone()
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
