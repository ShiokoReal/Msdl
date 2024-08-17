namespace Net.Myzuc.PurpleStainedGlass.Protocol.Blocks
{
    public sealed class BlockTubeCoral : Block
    {
        public override int BlockId => 12823 + (Waterlogged ? 0 : 1);
        public override int LiquidId => Waterlogged ? 1 : 0;
        public override int LightEmission => 0;
        public override int LightFilter => 1;
        public override (double xa, double ya, double za, double xb, double yb, double zb)[] Collisions => [];
        public bool Waterlogged = false;
        public BlockTubeCoral()
        {
            
        }
        public override BlockTubeCoral Clone()
        {
            return new()
            {
                Waterlogged = Waterlogged
            };
        }
        public override Block Break()
        {
            return Waterlogged ? new BlockWater() : new BlockAir();
        }
    }
}
