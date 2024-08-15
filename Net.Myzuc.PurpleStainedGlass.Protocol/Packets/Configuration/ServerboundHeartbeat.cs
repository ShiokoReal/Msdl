using Net.Myzuc.ShioLib;
using System.IO;

namespace Net.Myzuc.PurpleStainedGlass.Protocol.Packets.Configuration
{
    public sealed class ServerboundHeartbeat : Serverbound
    {
        public readonly long Sequence;
        internal ServerboundHeartbeat(MemoryStream stream)
        {
            Sequence = stream.ReadS64();
        }
    }
}
