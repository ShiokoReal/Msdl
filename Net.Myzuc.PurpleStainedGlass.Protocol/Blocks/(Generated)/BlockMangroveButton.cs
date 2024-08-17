namespace Net.Myzuc.PurpleStainedGlass.Protocol.Blocks
{
    public sealed class BlockMangroveButton : Block
    {
        public enum EnumFacing : int
        {
            North = 0,
            South = 1,
            West = 2,
            East = 3
        }
        public enum EnumFace : int
        {
            Floor = 0,
            Wall = 1,
            Ceiling = 2
        }
        public override int BlockId => 8779 + (Powered ? 0 : 1) + (int)Facing * 2 + (int)Face * 8;
        public override int LiquidId => 0;
        public override int LightEmission => 0;
        public override int LightFilter => 0;
        public override (double xa, double ya, double za, double xb, double yb, double zb)[] Collisions => [];
        public bool Powered = false;
        public EnumFacing Facing = EnumFacing.North;
        public EnumFace Face = EnumFace.Floor;
        public BlockMangroveButton()
        {
            
        }
        public override BlockMangroveButton Clone()
        {
            return new()
            {
                Powered = Powered,
                Facing = Facing,
                Face = Face
            };
        }
        public override Block Break()
        {
            return new BlockAir();
        }
    }
}
