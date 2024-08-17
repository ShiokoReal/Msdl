namespace Net.Myzuc.PurpleStainedGlass.Protocol.Blocks
{
    public sealed class BlockBubbleColumn : Block
    {
        public override int BlockId => 12960 + (Drag ? 0 : 1);
        public override int LiquidId => 0;
        public override int LightEmission => 0;
        public override int LightFilter => 1;
        public override (double xa, double ya, double za, double xb, double yb, double zb)[] Collisions => [];
        public bool Drag = false;
        public BlockBubbleColumn()
        {
            
        }
        public override BlockBubbleColumn Clone()
        {
            return new()
            {
                Drag = Drag
            };
        }
        public override Block Break()
        {
            return new BlockAir();
        }
    }
}
