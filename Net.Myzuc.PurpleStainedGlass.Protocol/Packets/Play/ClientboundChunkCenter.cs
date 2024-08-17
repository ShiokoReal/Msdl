using Net.Myzuc.PurpleStainedGlass.Protocol.Const;
using Net.Myzuc.ShioLib;
using System.IO;

namespace Net.Myzuc.PurpleStainedGlass.Protocol.Packets.Play
{
    public sealed class ClientboundChunkCenter : Clientbound
    {
        public override Client.EnumState Mode => Client.EnumState.Play;
        public ClientboundChunkCenter(int x, int z)
        {
            using MemoryStream stream = new();
            stream.WriteS32V(PacketIds.OutgoingPlayChunkCenter);
            stream.WriteS32V(x);
            stream.WriteS32V(z);
            Buffer = stream.ToArray();
        }
        internal override void Transform(Client client)
        {

        }
    }
}
