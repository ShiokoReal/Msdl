namespace Me.Shishioko.Msdl.Data
{
    public struct Position
    {
        public ulong Data { get; set; }
        public Position()
        {
            Data = 0;
        }
        public Position(ulong data)
        {
            Data = data;
        }
        public Position(int x, int y, int z)
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
                int Value = (int)(this.Data >> 38);
                if ((Value & 0x02000000) != 0) Value -= 0x04000000;
                return Value;
            }
            set
            {
                ulong Value = (ulong)value & 0x8000000001FFFFFFUL;
                this.Data &= 0x0000003FFFFFFFFFUL;
                if ((Value & 0x8000000000000000UL) != 0) Value |= 0x0000000002000000UL;
                this.Data |= (Value & 0x0000000003FFFFFFUL) << 38;
            }
        }
        public int Z
        {
            get
            {
                int Value = (int)(this.Data >> 12 & 0x3FFFFFF);
                if ((Value & 0x02000000) != 0) Value -= 0x4000000;
                return Value;
            }
            set
            {
                ulong Value = (ulong)value & 0x8000000001FFFFFFUL;
                this.Data &= 0xFFFFFFC000000FFFUL;
                if ((Value & 0x8000000000000000UL) != 0) Value |= 0x0000000002000000UL;
                this.Data |= (Value & 0x0000000003FFFFFFUL) << 12;
            }
        }
        public int Y
        {
            get
            {
                int Value = (int)(this.Data & 0xFFF);
                if ((Value & 0x00000800) != 0) Value -= 0x1000;
                return Value;
            }
            set
            {
                ulong Value = (ulong)value & 0x80000000000007FFUL;
                this.Data &= 0xFFFFFFFFFFFFF000UL;
                if ((Value & 0x8000000000000000UL) != 0) Value |= 0x0000000000000800UL;
                this.Data |= Value & 0x0000000000000FFFUL;
            }
        }
        public override int GetHashCode()
        {
            return Data.GetHashCode();
        }
        public override bool Equals(object? obj)
        {
            if (obj is not Position) return false;
            Position comparsion = (Position)obj;
            if (comparsion.Data != Data) return false;
            return true;
        }
        public static bool operator ==(Position left, Position right)
        {
            return left.Equals(right);
        }
        public static bool operator !=(Position left, Position right)
        {
            return !(left == right);
        }
    }
}
