using Net.Myzuc.PurpleStainedGlass.Protocol.Const;
using Net.Myzuc.PurpleStainedGlass.Protocol.Enums;
using Net.Myzuc.PurpleStainedGlass.Protocol.Structs;
using Net.Myzuc.ShioLib;
using System.IO;
using System.Net;

namespace Net.Myzuc.PurpleStainedGlass.Protocol.Packets.Play
{
    public sealed class ClientboundStart : Clientbound
    {
        public override Client.EnumState Mode => Client.EnumState.Play;
        private readonly int DimensionTypeId;
        public ClientboundStart(int eid, bool hardcore, string[] dimensions, int renderDistance, int simulationDistance, bool reducedDebug, bool respawnScreen, int registryDimensionId, string dimension, long seedHash, EnumGamemode gamemode, EnumGamemode? previousGamemode, bool debugWorld, bool flatWorld, (string dimension, Location location)? lastDeath, bool enforceSecureChat)
        {
            using MemoryStream stream = new();
            stream.WriteS32V(PacketIds.OutgoingPlayStart);
            stream.WriteS32(eid);
            stream.WriteBool(hardcore);
            stream.WriteS32V(dimensions.Length);
            for (int i = 0; i < dimensions.Length; i++) stream.WriteString(dimensions[i], SizePrefix.S32V);
            stream.WriteS32V(0);
            stream.WriteS32V(renderDistance);
            stream.WriteS32V(simulationDistance);
            stream.WriteBool(reducedDebug);
            stream.WriteBool(respawnScreen);
            stream.WriteBool(false);
            stream.WriteS32V(DimensionTypeId = registryDimensionId);
            stream.WriteString(dimension, SizePrefix.S32V);
            stream.WriteS64(seedHash);
            stream.WriteU8((byte)gamemode);
            stream.WriteU8((byte)(previousGamemode.HasValue ? (int)previousGamemode.Value : byte.MaxValue));
            stream.WriteBool(debugWorld);
            stream.WriteBool(flatWorld);
            stream.WriteBool(lastDeath.HasValue);
            if (lastDeath.HasValue)
            {
                stream.WriteString(lastDeath.Value.dimension, SizePrefix.S32V);
                stream.WriteU64(lastDeath.Value.location.Data);
            }
            stream.WriteS32V(0);
            stream.WriteBool(enforceSecureChat);
            Buffer = stream.ToArray();
        }
        internal override void Transform(Client client)
        {
            if (DimensionTypeId >= client.DimensionRegistry.Length) throw new ProtocolViolationException();
            client.Dimension = client.DimensionRegistry[DimensionTypeId];
        }
    }
}
