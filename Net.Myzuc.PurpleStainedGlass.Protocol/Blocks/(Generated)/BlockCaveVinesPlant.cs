namespace Net.Myzuc.PurpleStainedGlass.Protocol.Blocks
{
    public sealed class BlockCaveVinesPlant : Block
    {
        public override int BlockId => 24821 + (Berries ? 0 : 1);
        public override int LiquidId => 0;
        public override int LightEmission => 0;
        public override int LightFilter => 0;
        public override (double xa, double ya, double za, double xb, double yb, double zb)[] Collisions => [];
        public bool Berries = false;
        public BlockCaveVinesPlant()
        {
            
        }
        public override BlockCaveVinesPlant Clone()
        {
            return new()
            {
                Berries = Berries
            };
        }
        public override Block Break()
        {
            return new BlockAir();
        }
    }
}
