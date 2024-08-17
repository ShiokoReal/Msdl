namespace Net.Myzuc.PurpleStainedGlass.Protocol.Blocks
{
    public sealed class BlockRedstoneOre : Block
    {
        public override int BlockId => 5734 + (Lit ? 0 : 1);
        public override int LiquidId => 0;
        public override int LightEmission => 0;
        public override int LightFilter => 15;
        public override (double xa, double ya, double za, double xb, double yb, double zb)[] Collisions => [
            (0, 0, 0, 1, 1, 1)
        ];
        public bool Lit = false;
        public BlockRedstoneOre()
        {
            
        }
        public override BlockRedstoneOre Clone()
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
