using System;

namespace Net.Myzuc.PurpleStainedGlass.Protocol.Enums
{
    [Flags]
    public enum FlagsSkin : byte
    {
        Cape = 0x01,
        Body = 0x02,
        ArmLeft = 0x04,
        ArmRight = 0x08,
        LegLeft = 0x10,
        LegRight = 0x20,
        Hat = 0x40,
    }
}
