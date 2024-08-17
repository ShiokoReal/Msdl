namespace Net.Myzuc.PurpleStainedGlass.Protocol.Blocks
{
    public sealed class BlockRedstoneTorch : Block
    {
        public override int BlockId => 5738 + (Lit ? 0 : 1);
        public override int LiquidId => 0;
        public override int LightEmission => 7;
        public override int LightFilter => 0;
        public override (double xa, double ya, double za, double xb, double yb, double zb)[] Collisions => [];
        public bool Lit = false;
        public BlockRedstoneTorch()
        {
            
        }
        public override BlockRedstoneTorch Clone()
        {
            return new()
            {
                Lit = Lit
            };
        }
        public override Block Break()
        {
            return new BlockAir();
        }
    }
}
