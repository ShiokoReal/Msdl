using Net.Myzuc.PurpleStainedGlass.Protocol.Objects.Chat;
using Net.Myzuc.ShioLib;
using System.IO;

namespace Net.Myzuc.PurpleStainedGlass.Protocol.Packets.Login
{
    public sealed class ClientboundDisconnect : Clientbound
    {
        private const int Id = 0x00;
        public override Client.EnumState Mode => Client.EnumState.Login;
        public ClientboundDisconnect(ChatComponent message)
        {
            using MemoryStream stream = new();
            stream.WriteS32V(Id);
            stream.WriteString(message.JsonSerialize(), SizePrefix.S32V);
            Buffer = stream.ToArray();
        }
        internal override void Transform(Client client)
        {
            client.Dispose();
        }
    }
}
