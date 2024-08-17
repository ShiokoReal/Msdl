namespace Net.Myzuc.PurpleStainedGlass.Protocol.Blocks
{
    public sealed class BlockSculkShrieker : Block
    {
        public override int BlockId => 22930 + (Waterlogged ? 0 : 1) + (Shrieking ? 0 : 2) + (CanSummon ? 0 : 4);
        public override int LiquidId => Waterlogged ? 1 : 0;
        public override int LightEmission => 0;
        public override int LightFilter => 1;
        public override (double xa, double ya, double za, double xb, double yb, double zb)[] Collisions => [
            (0, 0, 0, 1, 0.5, 1)
        ];
        public bool Waterlogged = false;
        public bool Shrieking = false;
        public bool CanSummon = false;
        public BlockSculkShrieker()
        {
            
        }
        public override BlockSculkShrieker Clone()
        {
            return new()
            {
                Waterlogged = Waterlogged,
                Shrieking = Shrieking,
                CanSummon = CanSummon
            };
        }
        public override Block Break()
        {
            return Waterlogged ? new BlockWater() : new BlockAir();
        }
    }
}
