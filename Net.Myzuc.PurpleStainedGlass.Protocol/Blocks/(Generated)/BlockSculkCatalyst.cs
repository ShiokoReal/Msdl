namespace Net.Myzuc.PurpleStainedGlass.Protocol.Blocks
{
    public sealed class BlockSculkCatalyst : Block
    {
        public override int BlockId => 22928 + (Bloom ? 0 : 1);
        public override int LiquidId => 0;
        public override int LightEmission => 6;
        public override int LightFilter => 15;
        public override (double xa, double ya, double za, double xb, double yb, double zb)[] Collisions => [
            (0, 0, 0, 1, 1, 1)
        ];
        public bool Bloom = false;
        public BlockSculkCatalyst()
        {
            
        }
        public override BlockSculkCatalyst Clone()
        {
            return new()
            {
                Bloom = Bloom
            };
        }
        public override Block Break()
        {
            return new BlockAir();
        }
    }
}
