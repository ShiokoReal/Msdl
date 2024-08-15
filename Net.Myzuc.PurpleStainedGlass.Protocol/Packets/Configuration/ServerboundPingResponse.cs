using Net.Myzuc.ShioLib;
using System.IO;

namespace Net.Myzuc.PurpleStainedGlass.Protocol.Packets.Configuration
{
    public sealed class ServerboundPingResponse : Serverbound
    {
        public readonly int Sequence;
        internal ServerboundPingResponse(MemoryStream stream)
        {
            Sequence = stream.ReadS32();
        }
    }
}

