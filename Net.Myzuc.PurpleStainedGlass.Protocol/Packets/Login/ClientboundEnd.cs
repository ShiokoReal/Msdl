using Net.Myzuc.ShioLib;
using System;
using System.IO;

namespace Net.Myzuc.PurpleStainedGlass.Protocol.Packets.Login
{
    public sealed class ClientboundEnd : Clientbound
    {
        private const int Id = 0x02;
        public override Client.EnumState Mode => Client.EnumState.Login;
        public ClientboundEnd(Guid guid, string name, (string name, string value, string? signature)[] properties)
        {
            using MemoryStream stream = new();
            stream.WriteS32V(Id);
            stream.WriteGuid(guid);
            stream.WriteString(name, SizePrefix.S32V, 16);
            stream.WriteS32V(properties.Length);
            for (int i = 0; i < properties.Length; i++)
            {
                (string name, string value, string? signature) propety = properties[i];
                stream.WriteString(propety.name, SizePrefix.S32V, 32767);
                stream.WriteString(propety.value, SizePrefix.S32V, 32767);
                stream.WriteBool(propety.signature is not null);
                if (propety.signature is not null) stream.WriteString(propety.signature, SizePrefix.S32V, 32767);
            }
            stream.WriteBool(true);
            Buffer = stream.ToArray();
        }
        internal override void Transform(Client client)
        {

        }
    }
}
