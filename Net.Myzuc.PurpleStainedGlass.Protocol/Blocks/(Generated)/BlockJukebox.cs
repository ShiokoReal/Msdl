namespace Net.Myzuc.PurpleStainedGlass.Protocol.Blocks
{
    public sealed class BlockJukebox : Block
    {
        public override int BlockId => 5815 + (HasRecord ? 0 : 1);
        public override int LiquidId => 0;
        public override int LightEmission => 0;
        public override int LightFilter => 15;
        public override (double xa, double ya, double za, double xb, double yb, double zb)[] Collisions => [
            (0, 0, 0, 1, 1, 1)
        ];
        public bool HasRecord = false;
        public BlockJukebox()
        {
            
        }
        public override BlockJukebox Clone()
        {
            return new()
            {
                HasRecord = HasRecord
            };
        }
        public override Block Break()
        {
            return new BlockAir();
        }
    }
}
