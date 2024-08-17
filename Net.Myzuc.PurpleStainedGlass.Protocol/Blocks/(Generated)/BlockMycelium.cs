namespace Net.Myzuc.PurpleStainedGlass.Protocol.Blocks
{
    public sealed class BlockMycelium : Block
    {
        public override int BlockId => 7269 + (Snowy ? 0 : 1);
        public override int LiquidId => 0;
        public override int LightEmission => 0;
        public override int LightFilter => 15;
        public override (double xa, double ya, double za, double xb, double yb, double zb)[] Collisions => [
            (0, 0, 0, 1, 1, 1)
        ];
        public bool Snowy = false;
        public BlockMycelium()
        {
            
        }
        public override BlockMycelium Clone()
        {
            return new()
            {
                Snowy = Snowy
            };
        }
        public override Block Break()
        {
            return new BlockAir();
        }
    }
}
