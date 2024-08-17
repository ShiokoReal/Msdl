namespace Net.Myzuc.PurpleStainedGlass.Protocol.Blocks
{
    public sealed class BlockEnchantingTable : Block
    {
        public override int BlockId => 7389;
        public override int LiquidId => 0;
        public override int LightEmission => 7;
        public override int LightFilter => 0;
        public override (double xa, double ya, double za, double xb, double yb, double zb)[] Collisions => [
            (0, 0, 0, 1, 0.75, 1)
        ];
        public BlockEnchantingTable()
        {
            
        }
        public override BlockEnchantingTable Clone()
        {
            return new();
        }
        public override Block Break()
        {
            return new BlockAir();
        }
    }
}
