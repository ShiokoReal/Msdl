using Net.Myzuc.ShioLib;
using System.IO;

namespace Net.Myzuc.PurpleStainedGlass.Protocol.Packets.Configuration
{
    public sealed class ClientboundHeartbeat : Clientbound
    {
        private const int Id = 0x04;
        public override Client.EnumState Mode => Client.EnumState.Configuration;
        public ClientboundHeartbeat(long sequence)
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
