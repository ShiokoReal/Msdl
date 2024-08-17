namespace Net.Myzuc.PurpleStainedGlass.Protocol.Blocks
{
    public sealed class BlockChiseledBookshelf : Block
    {
        public enum EnumFacing : int
        {
            North = 0,
            South = 1,
            West = 2,
            East = 3
        }
        public override int BlockId => 2097 + (Slot5Occupied ? 0 : 1) + (Slot4Occupied ? 0 : 2) + (Slot3Occupied ? 0 : 4) + (Slot2Occupied ? 0 : 8) + (Slot1Occupied ? 0 : 16) + (Slot0Occupied ? 0 : 32) + (int)Facing * 64;
        public override int LiquidId => 0;
        public override int LightEmission => 0;
        public override int LightFilter => 15;
        public override (double xa, double ya, double za, double xb, double yb, double zb)[] Collisions => [
            (0, 0, 0, 1, 1, 1)
        ];
        public bool Slot5Occupied = false;
        public bool Slot4Occupied = false;
        public bool Slot3Occupied = false;
        public bool Slot2Occupied = false;
        public bool Slot1Occupied = false;
        public bool Slot0Occupied = false;
        public EnumFacing Facing = EnumFacing.North;
        public BlockChiseledBookshelf()
        {
            
        }
        public override BlockChiseledBookshelf Clone()
        {
            return new()
            {
                Slot5Occupied = Slot5Occupied,
                Slot4Occupied = Slot4Occupied,
                Slot3Occupied = Slot3Occupied,
                Slot2Occupied = Slot2Occupied,
                Slot1Occupied = Slot1Occupied,
                Slot0Occupied = Slot0Occupied,
                Facing = Facing
            };
        }
        public override Block Break()
        {
            return new BlockAir();
        }
    }
}
