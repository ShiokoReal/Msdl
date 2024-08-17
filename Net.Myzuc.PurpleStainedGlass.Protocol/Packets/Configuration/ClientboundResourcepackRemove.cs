using Net.Myzuc.ShioLib;
using System;
using System.IO;

namespace Net.Myzuc.PurpleStainedGlass.Protocol.Packets.Configuration
{
    public sealed class ClientboundResourcepackRemove : Clientbound
    {
        private const int Id = 0x08;
        public override Client.EnumState Mode => Client.EnumState.Configuration;
        public ClientboundResourcepackRemove(Guid? guid)
        {
            using MemoryStream stream = new();
            stream.WriteS32V(Id);
            stream.WriteBool(guid.HasValue);
            if (guid is not null) stream.WriteGuid(guid.Value);
            Buffer = stream.ToArray();
        }
        internal override void Transform(Client client)
        {

        }
    }
}
