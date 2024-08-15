using Net.Myzuc.ShioLib;
using System.IO;

namespace Net.Myzuc.PurpleStainedGlass.Protocol.Packets.Configuration
{
    public sealed class ClientboundPingRequest : Clientbound
    {
        private const int Id = 0x05;
        public override Client.EnumState Mode => Client.EnumState.Configuration;
        public ClientboundPingRequest(int sequence)
        {
            using MemoryStream stream = new();
            stream.WriteS32V(Id);
            stream.WriteS32(sequence);
            Buffer = stream.ToArray();
        }
        internal override void Transform(Client client)
        {

        }
    }
}
