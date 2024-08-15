using Net.Myzuc.ShioLib;
using System.IO;

namespace Net.Myzuc.PurpleStainedGlass.Protocol.Packets.Status
{
    public sealed class ClientboundStatusResponse : Clientbound
    {
        private const int Id = 0x00;
        public override Client.EnumState Mode => Client.EnumState.Status;
        public ClientboundStatusResponse(Objects.Status status)
        {
            using MemoryStream stream = new();
            stream.WriteS32V(Id);
            stream.WriteString(status.TextSerialize(), SizePrefix.S32V);
            Buffer = stream.ToArray();
        }
        internal override void Transform(Client client)
        {

        }
    }
}
