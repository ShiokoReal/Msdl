namespace Net.Myzuc.PurpleStainedGlass.Protocol.Blocks
{
    public sealed class BlockTripwire : Block
    {
        public override int BlockId => 7537 + (West ? 0 : 1) + (South ? 0 : 2) + (Powered ? 0 : 4) + (North ? 0 : 8) + (East ? 0 : 16) + (Disarmed ? 0 : 32) + (Attached ? 0 : 64);
        public override int LiquidId => 0;
        public override int LightEmission => 0;
        public override int LightFilter => 0;
        public override (double xa, double ya, double za, double xb, double yb, double zb)[] Collisions => [];
        public bool West = false;
        public bool South = false;
        public bool Powered = false;
        public bool North = false;
        public bool East = false;
        public bool Disarmed = false;
        public bool Attached = false;
        public BlockTripwire()
        {
            
        }
        public override BlockTripwire Clone()
        {
            return new()
            {
                West = West,
                South = South,
                Powered = Powered,
                North = North,
                East = East,
                Disarmed = Disarmed,
                Attached = Attached
            };
        }
        public override Block Break()
        {
            return new BlockAir();
        }
    }
}
