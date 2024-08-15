using Net.Myzuc.ShioLib;
using System.IO;

namespace Net.Myzuc.PurpleStainedGlass.Protocol.Packets.Login
{
    public sealed class ServerboundCustomResponse : Serverbound
    {
        public readonly int Sequence;
        public readonly bool Success;
        public readonly byte[] Data;
        internal ServerboundCustomResponse(MemoryStream stream)
        {
            Sequence = stream.ReadS32V();
            Success = stream.ReadBool();
            Data = stream.ReadU8A((int)(stream.Length - stream.Position), 1048576);
        }
    }
}
