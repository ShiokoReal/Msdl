using Net.Myzuc.PurpleStainedGlass.Protocol.Objects.Chat;
using Net.Myzuc.ShioLib;
using System.IO;

namespace Net.Myzuc.PurpleStainedGlass.Protocol.Packets.Configuration
{
    public sealed class ClientboundDisconnect : Clientbound
    {
        private const int Id = 0x02;
        public override Client.EnumState Mode => Client.EnumState.Configuration;
        public ClientboundDisconnect(ChatComponent message)
        {
            using MemoryStream stream = new();
            stream.WriteS32V(Id);
            message.NbtSerialize(stream);
            Buffer = stream.ToArray();
        }
        internal override void Transform(Client client)
        {
            client.Dispose();
        }
    }
}
