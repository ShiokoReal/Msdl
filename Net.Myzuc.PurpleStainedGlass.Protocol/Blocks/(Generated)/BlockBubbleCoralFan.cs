namespace Net.Myzuc.PurpleStainedGlass.Protocol.Blocks
{
    public sealed class BlockBubbleCoralFan : Block
    {
        public override int BlockId => 12847 + (Waterlogged ? 0 : 1);
        public override int LiquidId => Waterlogged ? 1 : 0;
        public override int LightEmission => 0;
        public override int LightFilter => 1;
        public override (double xa, double ya, double za, double xb, double yb, double zb)[] Collisions => [];
        public bool Waterlogged = false;
        public BlockBubbleCoralFan()
        {
            
        }
        public override BlockBubbleCoralFan Clone()
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
