using System;

namespace Net.Myzuc.PurpleStainedGlass.Protocol.Enums
{
    [Flags]
    public enum FlagsEntityLiving : byte
    {
        HandActive = 0x01,
        ActiveHandOffhand = 0x02,
        Spinning = 0x04,
    }
}
