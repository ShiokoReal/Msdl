using Net.Myzuc.ShioLib;
using System.IO;
using System.Net;
using System.Security.Cryptography;

namespace Net.Myzuc.PurpleStainedGlass.Protocol.Packets.Login
{
    public sealed class ServerboundEncryptionResponse : Serverbound
    {
        public readonly byte[] Secret;
        public readonly byte[] Verification;
        public ServerboundEncryptionResponse(MemoryStream stream, Client client)
        {
            if (client.KeyExchange is null) throw new ProtocolViolationException();
            Secret = client.KeyExchange.Decrypt(stream.ReadU8A(SizePrefix.S32V), RSAEncryptionPadding.Pkcs1);
            Verification = client.KeyExchange.Decrypt(stream.ReadU8A(SizePrefix.S32V), RSAEncryptionPadding.Pkcs1);
            if (stream.Position != stream.Length) throw new ProtocolViolationException();
            client.Stream = new AesCfbStream(client.Stream, Secret, Secret, false);
        }
    }
}
