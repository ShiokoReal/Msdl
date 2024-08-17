namespace Net.Myzuc.PurpleStainedGlass.Protocol.Blocks
{
    public sealed class BlockTnt : Block
    {
        public override int BlockId => 2094 + (Unstable ? 0 : 1);
        public override int LiquidId => 0;
        public override int LightEmission => 0;
        public override int LightFilter => 15;
        public override (double xa, double ya, double za, double xb, double yb, double zb)[] Collisions => [
            (0, 0, 0, 1, 1, 1)
        ];
        public bool Unstable = false;
        public BlockTnt()
        {
            
        }
        public override BlockTnt Clone()
        {
            return new()
            {
                Unstable = Unstable
            };
        }
        public override Block Break()
        {
            return new BlockAir();
        }
    }
}
