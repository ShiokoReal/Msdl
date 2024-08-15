namespace Net.Myzuc.PurpleStainedGlass.Protocol.Structs
{
    public struct Location
    {
        public ulong Data { get; set; }
        public Location()
        {
            Data = 0;
        }
        public Location(ulong data)
        {
            Data = data;
        }
        public Location(int x, int y, int z)
        {
            Data = 0;
            X = x;
            Y = y;
            Z = z;
        }
        public int X
        {
            get
            {
                int Value = (int)(Data >> 38);
                if ((Value & 0x02000000) != 0) Value -= 0x04000000;
                return Value;
            }
            set
            {
                ulong Value = (ulong)value & 0x8000000001FFFFFFUL;
                Data &= 0x0000003FFFFFFFFFUL;
                if ((Value & 0x8000000000000000UL) != 0) Value |= 0x0000000002000000UL;
                Data |= (Value & 0x0000000003FFFFFFUL) << 38;
            }
        }
        public int Z
        {
            get
            {
                int Value = (int)(Data >> 12 & 0x3FFFFFF);
                if ((Value & 0x02000000) != 0) Value -= 0x4000000;
                return Value;
            }
            set
            {
                ulong Value = (ulong)value & 0x8000000001FFFFFFUL;
                Data &= 0xFFFFFFC000000FFFUL;
                if ((Value & 0x8000000000000000UL) != 0) Value |= 0x0000000002000000UL;
                Data |= (Value & 0x0000000003FFFFFFUL) << 12;
            }
        }
        public int Y
        {
            get
            {
                int Value = (int)(Data & 0xFFF);
                if ((Value & 0x00000800) != 0) Value -= 0x1000;
                return Value;
            }
            set
            {
                ulong Value = (ulong)value & 0x80000000000007FFUL;
                Data &= 0xFFFFFFFFFFFFF000UL;
                if ((Value & 0x8000000000000000UL) != 0) Value |= 0x0000000000000800UL;
                Data |= Value & 0x0000000000000FFFUL;
            }
        }
        public override int GetHashCode()
        {
            return Data.GetHashCode();
        }
        public override bool Equals(object? obj)
        {
            if (obj is not Location) return false;
            Location comparsion = (Location)obj;
            if (comparsion.Data != Data) return false;
            return true;
        }
        public static bool operator ==(Location left, Location right)
        {
            return left.Equals(right);
        }
        public static bool operator !=(Location left, Location right)
        {
            return !(left == right);
        }
    }
}
