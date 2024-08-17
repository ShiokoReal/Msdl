namespace Net.Myzuc.PurpleStainedGlass.Protocol.Blocks
{
    public sealed class BlockDropper : Block
    {
        public enum EnumFacing : int
        {
            North = 0,
            East = 1,
            South = 2,
            West = 3,
            Up = 4,
            Down = 5
        }
        public override int BlockId => 9344 + (Triggered ? 0 : 1) + (int)Facing * 2;
        public override int LiquidId => 0;
        public override int LightEmission => 0;
        public override int LightFilter => 15;
        public override (double xa, double ya, double za, double xb, double yb, double zb)[] Collisions => [
            (0, 0, 0, 1, 1, 1)
        ];
        public bool Triggered = false;
        public EnumFacing Facing = EnumFacing.North;
        public BlockDropper()
        {
            
        }
        public override BlockDropper Clone()
        {
            return new()
            {
                Triggered = Triggered,
                Facing = Facing
            };
        }
        public override Block Break()
        {
            return new BlockAir();
        }
    }
}
