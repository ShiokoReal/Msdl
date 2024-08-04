namespace Me.Shishioko.Msdl.Data
{
    public struct SkinMask
    {
        public static readonly SkinMask Full = new(0xFF);
        public byte Mask;
        public bool Cape
        {
            get => (Mask & 0x01) != 0;
            set => Mask = (byte)((Mask & ~0x01) | (value ? 0x01 : 0x00));
        }
        public bool Body
        {
            get => (Mask & 0x02) != 0;
            set => Mask = (byte)((Mask & ~0x02) | (value ? 0x02 : 0x00));
        }
        public bool ArmLeft
        {
            get => (Mask & 0x04) != 0;
            set => Mask = (byte)((Mask & ~0x04) | (value ? 0x04 : 0x00));
        }
        public bool ArmRight
        {
            get => (Mask & 0x08) != 0;
            set => Mask = (byte)((Mask & ~0x08) | (value ? 0x08 : 0x00));
        }
        public bool LegLeft
        {
            get => (Mask & 0x10) != 0;
            set => Mask = (byte)((Mask & ~0x10) | (value ? 0x10 : 0x00));
        }
        public bool LegRight
        {
            get => (Mask & 0x20) != 0;
            set => Mask = (byte)((Mask & ~0x20) | (value ? 0x20 : 0x00));
        }
        public bool Hat
        {
            get => (Mask & 0x40) != 0;
            set => Mask = (byte)((Mask & ~0x40) | (value ? 0x40 : 0x00));
        }
        public SkinMask()
        {
            Mask = 0x7F;
        }
        public SkinMask(byte mask)
        {
            Mask = mask;
        }
    }
}
