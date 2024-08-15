using Net.Myzuc.ShioLib;
using System.IO;

namespace Net.Myzuc.PurpleStainedGlass.Protocol.Packets.Login
{
    public sealed class ClientboundCookieRequest : Clientbound
    {
        private const int Id = 0x05;
        public override Client.EnumState Mode => Client.EnumState.Login;
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
