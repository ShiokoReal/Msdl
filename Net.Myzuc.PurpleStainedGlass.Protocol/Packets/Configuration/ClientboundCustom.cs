using Net.Myzuc.ShioLib;
using System.IO;

namespace Net.Myzuc.PurpleStainedGlass.Protocol.Packets.Configuration
{
    public sealed class ClientboundCustom : Clientbound
    {
        private const int Id = 0x01;
        public override Client.EnumState Mode => Client.EnumState.Configuration;
        public ClientboundCustom(string channel, byte[] data)
        {
            using MemoryStream stream = new();
            stream.WriteS32V(Id);
            stream.WriteString(channel, SizePrefix.S32V);
            stream.WriteU8A(data, 1048576);
            Buffer = stream.ToArray();
        }
        internal override void Transform(Client client)
        {

        }
    }
}
