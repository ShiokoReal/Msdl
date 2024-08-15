using Net.Myzuc.PurpleStainedGlass.Protocol.Data.Chat;
using Net.Myzuc.ShioLib;
using System;
using System.IO;
using System.Linq;

namespace Net.Myzuc.PurpleStainedGlass.Protocol.Packets.Configuration
{
    public sealed class ClientboundTransfer : Clientbound
    {
        private const int Id = 0x0B;
        public override Client.EnumState Mode => Client.EnumState.Configuration;
        public ClientboundTransfer(string destination, ushort port)
        {
            using MemoryStream stream = new();
            stream.WriteS32V(Id);
            stream.WriteString(destination, SizePrefix.S32V);
            stream.WriteS32V(port);
            Buffer = stream.ToArray();
        }
        internal override void Transform(Client client)
        {
            client.Dispose();
        }
    }
}
