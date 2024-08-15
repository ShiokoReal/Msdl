using Net.Myzuc.ShioLib;
using System.IO;

namespace Net.Myzuc.PurpleStainedGlass.Protocol.Packets.Login
{
    public sealed class ServerboundCookieResponse : Serverbound
    {
        public readonly string Identifier;
        public readonly byte[]? Data;
        internal ServerboundCookieResponse(MemoryStream stream)
        {
            Identifier = stream.ReadString(SizePrefix.S32V);
            Data = stream.ReadBool() ? stream.ReadU8A(SizePrefix.S32V, 5120) : null;
        }
    }
}
