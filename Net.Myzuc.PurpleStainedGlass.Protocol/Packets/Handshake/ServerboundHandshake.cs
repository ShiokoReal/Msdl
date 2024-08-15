using Net.Myzuc.ShioLib;
using System.IO;
using System.Net;

namespace Net.Myzuc.PurpleStainedGlass.Protocol.Packets.Handshake
{
    public sealed class ServerboundHandshake : Serverbound
    {
        public readonly int Version;
        public readonly string OriginAddress;
        public readonly ushort OriginPort;
        public readonly Client.EnumState State;
        internal ServerboundHandshake(MemoryStream stream, Client client) : base()
        {
            Version = stream.ReadS32V();
            OriginAddress = stream.ReadString(SizePrefix.S32V, 255);
            OriginPort = stream.ReadU16();
            int state = stream.ReadS32V();
            if (state == 2) State = Client.EnumState.Login;
            else if (state == 1) State = Client.EnumState.Status;
            else throw new ProtocolViolationException();
            client.State = State;
        }
    }
}
