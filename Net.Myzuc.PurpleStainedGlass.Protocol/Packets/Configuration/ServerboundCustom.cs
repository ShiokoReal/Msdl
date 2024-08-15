using Net.Myzuc.ShioLib;
using System.IO;

namespace Net.Myzuc.PurpleStainedGlass.Protocol.Packets.Configuration
{
    public sealed class ServerboundCustom : Serverbound
    {
        public readonly string Channel;
        public readonly byte[] Data;
        internal ServerboundCustom(MemoryStream stream)
        {
            Channel = stream.ReadString(SizePrefix.S32V);
            Data = stream.ReadU8A((int)(stream.Length - stream.Position), 1048576);
        }
    }
}
