using Net.Myzuc.PurpleStainedGlass.Protocol.Const;
using Net.Myzuc.PurpleStainedGlass.Protocol.Structs;
using Net.Myzuc.ShioLib;
using System.IO;

namespace Net.Myzuc.PurpleStainedGlass.Protocol.Packets.Play
{
    public sealed class ClientboundSpawnpoint : Clientbound
    {
        public override Client.EnumState Mode => Client.EnumState.Play;
        public ClientboundSpawnpoint(Location location, float angle)
        {
            using MemoryStream stream = new();
            stream.WriteS32V(PacketIds.OutgoingPlaySpawnpoint);
            stream.WriteU64(location.Data);
            stream.WriteF32(angle);
            Buffer = stream.ToArray();
        }
        internal override void Transform(Client client)
        {

        }
    }
}
