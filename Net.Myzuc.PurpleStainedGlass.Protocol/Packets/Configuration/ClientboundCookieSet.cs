using Net.Myzuc.ShioLib;
using System.IO;

namespace Net.Myzuc.PurpleStainedGlass.Protocol.Packets.Configuration
{
    public sealed class ClientboundCookieSet : Clientbound
    {
        private const int Id = 0x0A;
        public override Client.EnumState Mode => Client.EnumState.Configuration;
        public ClientboundCookieSet(string identifier, byte[] data)
        {
            using MemoryStream stream = new();
            stream.WriteS32V(Id);
            stream.WriteString(identifier, SizePrefix.S32V);
            stream.WriteU8A(data, SizePrefix.S32V, 5120);
            Buffer = stream.ToArray();
        }
        internal override void Transform(Client client)
        {

        }
    }
}
