namespace Net.Myzuc.PurpleStainedGlass.Protocol.Blocks
{
    public sealed class BlockTripwireHook : Block
    {
        public enum EnumFacing : int
        {
            North = 0,
            South = 1,
            West = 2,
            East = 3
        }
        public override int BlockId => 7521 + (Powered ? 0 : 1) + (int)Facing * 2 + (Attached ? 0 : 8);
        public override int LiquidId => 0;
        public override int LightEmission => 0;
        public override int LightFilter => 0;
        public override (double xa, double ya, double za, double xb, double yb, double zb)[] Collisions => [];
        public bool Powered = false;
        public EnumFacing Facing = EnumFacing.North;
        public bool Attached = false;
        public BlockTripwireHook()
        {
            
        }
        public override BlockTripwireHook Clone()
        {
            return new()
            {
                Powered = Powered,
                Facing = Facing,
                Attached = Attached
            };
        }
        public override Block Break()
        {
            return new BlockAir();
        }
    }
}
