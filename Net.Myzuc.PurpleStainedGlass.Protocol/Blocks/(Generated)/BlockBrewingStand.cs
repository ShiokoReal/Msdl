namespace Net.Myzuc.PurpleStainedGlass.Protocol.Blocks
{
    public sealed class BlockBrewingStand : Block
    {
        public override int BlockId => 7390 + (HasBottle2 ? 0 : 1) + (HasBottle1 ? 0 : 2) + (HasBottle0 ? 0 : 4);
        public override int LiquidId => 0;
        public override int LightEmission => 1;
        public override int LightFilter => 0;
        public override (double xa, double ya, double za, double xb, double yb, double zb)[] Collisions => [
            (0.0625, 0, 0.0625, 0.9375, 0.125, 0.9375),
            (0.4375, 0.125, 0.4375, 0.5625, 0.875, 0.5625)
        ];
        public bool HasBottle2 = false;
        public bool HasBottle1 = false;
        public bool HasBottle0 = false;
        public BlockBrewingStand()
        {
            
        }
        public override BlockBrewingStand Clone()
        {
            return new()
            {
                HasBottle2 = HasBottle2,
                HasBottle1 = HasBottle1,
                HasBottle0 = HasBottle0
            };
        }
        public override Block Break()
        {
            return new BlockAir();
        }
    }
}
