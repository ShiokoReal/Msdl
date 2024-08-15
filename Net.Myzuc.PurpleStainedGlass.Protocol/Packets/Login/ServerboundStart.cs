using Net.Myzuc.ShioLib;
using System;
using System.IO;

namespace Net.Myzuc.PurpleStainedGlass.Protocol.Packets.Login
{
    public sealed class ServerboundStart : Serverbound
    {
        public readonly Guid Guid;
        public readonly string Name;
        internal ServerboundStart(MemoryStream stream)
        {
            Name = stream.ReadString(SizePrefix.S32V, 16);
            Guid = stream.ReadGuid();
        }
    }
}
