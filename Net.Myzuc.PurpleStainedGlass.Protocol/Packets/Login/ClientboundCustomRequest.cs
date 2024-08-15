using Net.Myzuc.ShioLib;
using System.IO;

namespace Net.Myzuc.PurpleStainedGlass.Protocol.Packets.Login
{
    public sealed class ClientboundCustomRequest : Clientbound
    {
        private const int Id = 0x04;
        public override Client.EnumState Mode => Client.EnumState.Login;
        public ClientboundCustomRequest(int sequence, string channel, byte[] data)
        {
            using MemoryStream stream = new();
            stream.WriteS32V(Id);
            stream.WriteS32V(sequence);
            stream.WriteString(channel, SizePrefix.S32V);
            stream.WriteU8A(data, 1048576);
            Buffer = stream.ToArray();
        }
        internal override void Transform(Client client)
        {

        }
    }
}
