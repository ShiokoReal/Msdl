using Net.Myzuc.ShioLib;
using System.IO;

namespace Net.Myzuc.PurpleStainedGlass.Protocol.Packets.Status
{
    public sealed class ServerboundPingRequest : Serverbound
    {
        public readonly long Sequence;
        internal ServerboundPingRequest(MemoryStream stream)
        {
            Sequence = stream.ReadS64();
        }
    }
}
