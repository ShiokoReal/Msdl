using Net.Myzuc.ShioLib;
using System;
using System.IO;
using System.Security.Cryptography;

namespace Net.Myzuc.PurpleStainedGlass.Protocol.Packets.Login
{
    public sealed class ClientboundEncryptionRequest : Clientbound
    {
        private const int Id = 0x01;
        public override Client.EnumState Mode => Client.EnumState.Login;
        private readonly RSA RSA;
        public ClientboundEncryptionRequest(string server, RSA rsa, byte[] verifcation, bool authenticate)
        {
            using MemoryStream stream = new();
            stream.WriteS32V(Id);
            stream.WriteString(server, SizePrefix.S32V, 20);
            stream.WriteU8A(rsa.ExportSubjectPublicKeyInfo(), SizePrefix.S32V);
            stream.WriteU8A(verifcation, SizePrefix.S32V);
            stream.WriteBool(authenticate);
            Buffer = stream.ToArray();
            RSA = rsa;
        }
        internal override void Transform(Client client)
        {
            if (client.KeyExchange is not null) throw new NotSupportedException();
            client.KeyExchange = RSA;
        }
    }
}
