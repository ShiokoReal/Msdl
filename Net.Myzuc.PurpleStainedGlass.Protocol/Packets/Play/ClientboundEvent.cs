using Net.Myzuc.PurpleStainedGlass.Protocol.Const;
using Net.Myzuc.PurpleStainedGlass.Protocol.Enums;
using Net.Myzuc.PurpleStainedGlass.Protocol.Structs;
using Net.Myzuc.ShioLib;
using System.IO;

namespace Net.Myzuc.PurpleStainedGlass.Protocol.Packets.Play
{
    public sealed class ClientboundEvent : Clientbound
    {
        public static ClientboundEvent RespawnFailure() => new(0, 0.0f);
        public static ClientboundEvent RainStart() => new(1, 0.0f);
        public static ClientboundEvent RainEnd() => new(2, 0.0f);
        public static ClientboundEvent Gamemode(EnumGamemode gamemode) => new(3, (float)gamemode);
        public static ClientboundEvent Win(bool credits) => new(4, credits ? 1.0f : 0.0f);
        public static ClientboundEvent DemoMessage(EnumDemoMessage message) => new(5, (float)message);
        public static ClientboundEvent RainStrength(float strength) => new(7, strength);
        public static ClientboundEvent ThunderStrength(float strength) => new(8, strength);
        public static ClientboundEvent PufferfishSting() => new(9, 0.0f);
        public static ClientboundEvent ElderGuardian() => new(10, 0.0f);
        public static ClientboundEvent RespawnScreen(bool respawnScreen) => new(11, respawnScreen ? 0.0f : 1.0f);
        public static ClientboundEvent ChunkLoad() => new(13, 0.0f);
        public override Client.EnumState Mode => Client.EnumState.Play;
        internal ClientboundEvent(byte @event, float data)
        {
            using MemoryStream stream = new();
            stream.WriteS32V(PacketIds.OutgoingPlayEvent);
            stream.WriteU8(@event);
            stream.WriteF32(data);
            Buffer = stream.ToArray();
        }
        internal override void Transform(Client client)
        {

        }
    }
}
