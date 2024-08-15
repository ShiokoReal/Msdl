using Net.Myzuc.ShioLib;
using System.IO;

namespace Net.Myzuc.PurpleStainedGlass.Protocol.Packets.Status
{
    public sealed class ClientboundPingResponse : Clientbound
    {
        private const int Id = 0x01;
        public override Client.EnumState Mode => Client.EnumState.Status;
        public ClientboundPingResponse(long sequence)
        {
            using MemoryStream stream = new();
            stream.WriteS32V(Id);
            stream.WriteS64(sequence);
            Buffer = stream.ToArray();
        }
        internal override void Transform(Client client)
        {

        }
    }
}
