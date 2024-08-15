using Net.Myzuc.ShioLib;
using System.IO;
using System.IO.Compression;
using System.Net;

namespace Net.Myzuc.PurpleStainedGlass.Protocol.Packets.Login
{
    public sealed class ClientboundCompression : Clientbound
    {
        private const int Id = 0x03;
        public override Client.EnumState Mode => Client.EnumState.Login;
        private readonly int CompressionThreshold;
        private readonly CompressionLevel CompressionLevel;
        public ClientboundCompression(int compressionThreshold, CompressionLevel compressionLevel)
        {
            using MemoryStream stream = new();
            stream.WriteS32V(Id);
            stream.WriteS32V(compressionThreshold);
            Buffer = stream.ToArray();
            CompressionThreshold = compressionThreshold;
            CompressionLevel = compressionLevel;
        }
        internal override void Transform(Client client)
        {
            client.CompressionThreshold = CompressionThreshold;
            client.CompressionLevel = CompressionLevel;
        }
    }
}
