using System;

namespace Net.Myzuc.PurpleStainedGlass.Protocol.Enums
{
    [Flags]
    public enum FlagsEntity : byte
    {
        Burning = 0x01,
        Sneaking = 0x02,
        Sprinting = 0x08,
        Swimming = 0x10,
        Invisible = 0x20,
        Glowing = 0x40,
        ElytraFlying = 0x80,
    }
}
