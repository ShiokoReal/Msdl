using Net.Myzuc.ShioLib;
using System.IO;

namespace Net.Myzuc.PurpleStainedGlass.Protocol.Packets.Configuration
{
    public sealed class ClientboundCookieRequest : Clientbound
    {
        private const int Id = 0x00;
        public override Client.EnumState Mode => Client.EnumState.Configuration;
        public ClientboundCookieRequest(string identifier)
        {
            using MemoryStream stream = new();
            stream.WriteS32V(Id);
            stream.WriteString(identifier, SizePrefix.S32V);
            Buffer = stream.ToArray();
        }
        internal override void Transform(Client client)
        {

        }
    }
}
