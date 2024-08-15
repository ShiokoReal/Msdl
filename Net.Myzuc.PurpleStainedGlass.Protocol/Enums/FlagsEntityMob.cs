using System;

namespace Net.Myzuc.PurpleStainedGlass.Protocol.Enums
{
    [Flags]
    public enum FlagsEntityMob
    {
        NoAI = 0x01,
        Lefthanded = 0x02,
        Aggressive = 0x04,
    }
}
