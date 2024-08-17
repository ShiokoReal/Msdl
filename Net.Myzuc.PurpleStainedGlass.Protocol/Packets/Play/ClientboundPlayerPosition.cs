using Net.Myzuc.PurpleStainedGlass.Protocol.Const;
using Net.Myzuc.PurpleStainedGlass.Protocol.Structs;
using Net.Myzuc.ShioLib;
using System.IO;

namespace Net.Myzuc.PurpleStainedGlass.Protocol.Packets.Play
{
    public sealed class ClientboundPlayerPosition : Clientbound
    {
        public override Client.EnumState Mode => Client.EnumState.Play;
        public ClientboundPlayerPosition(double x, double y, double z, float yaw, float pitch, int id)
        {
            using MemoryStream stream = new();
            stream.WriteS32V(PacketIds.OutgoingPlayPlayerPosition);
            stream.WriteF64(x);
            stream.WriteF64(y);
            stream.WriteF64(z);
            stream.WriteF32(yaw);
            stream.WriteF32(pitch);
            stream.WriteU8(0);
            stream.WriteS32V(id);
            Buffer = stream.ToArray();
        }
        internal override void Transform(Client client)
        {

        }
    }
}
