namespace Net.Myzuc.PurpleStainedGlass.Protocol.Blocks
{
    public sealed class BlockLargeFern : Block
    {
        public enum EnumHalf : int
        {
            Upper = 0,
            Lower = 1
        }
        public override int BlockId => 10757 + (int)Half * 1;
        public override int LiquidId => 0;
        public override int LightEmission => 0;
        public override int LightFilter => 0;
        public override (double xa, double ya, double za, double xb, double yb, double zb)[] Collisions => [];
        public EnumHalf Half = EnumHalf.Upper;
        public BlockLargeFern()
        {
            
        }
        public override BlockLargeFern Clone()
        {
            return new()
            {
                Half = Half
            };
        }
        public override Block Break()
        {
            return new BlockAir();
        }
    }
}