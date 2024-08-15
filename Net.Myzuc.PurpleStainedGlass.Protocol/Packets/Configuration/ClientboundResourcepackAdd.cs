using Net.Myzuc.PurpleStainedGlass.Protocol.Objects.Chat;
using Net.Myzuc.ShioLib;
using System;
using System.IO;
using System.Linq;

namespace Net.Myzuc.PurpleStainedGlass.Protocol.Packets.Configuration
{
    public sealed class ClientboundResourcepackAdd : Clientbound
    {
        private const int Id = 0x09;
        public override Client.EnumState Mode => Client.EnumState.Configuration;
        public ClientboundResourcepackAdd(Guid guid, string url, byte[] sha1, bool required, ChatComponent? prompt)
        {
            if (sha1.Length != 20) throw new ArgumentException();
            using MemoryStream stream = new();
            stream.WriteS32V(Id);
            stream.WriteGuid(guid);
            stream.WriteString(url, SizePrefix.S32V, 32767);
            stream.WriteString(string.Concat(sha1.Select(b => b.ToString("X02"))), SizePrefix.S32V, 32767);
            stream.WriteBool(required);
            stream.WriteBool(prompt is not null);
            if (prompt is not null) prompt.NbtSerialize(stream);
            Buffer = stream.ToArray();
        }
        internal override void Transform(Client client)
        {

        }
    }
}
