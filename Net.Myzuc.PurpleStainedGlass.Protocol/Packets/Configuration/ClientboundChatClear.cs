using Net.Myzuc.ShioLib;
using System.IO;

namespace Net.Myzuc.PurpleStainedGlass.Protocol.Packets.Configuration
{
    public sealed class ClientboundChatClear : Clientbound
    {
        private const int Id = 0x06;
        public override Client.EnumState Mode => Client.EnumState.Configuration;
        public ClientboundChatClear()
        {
            using MemoryStream stream = new();
            stream.WriteS32V(Id);
            Buffer = stream.ToArray();
        }
        internal override void Transform(Client client)
        {

        }
    }
}
