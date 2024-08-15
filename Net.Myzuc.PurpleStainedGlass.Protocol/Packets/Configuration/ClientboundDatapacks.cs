using Net.Myzuc.ShioLib;
using System.IO;

namespace Net.Myzuc.PurpleStainedGlass.Protocol.Packets.Configuration
{
    public sealed class ClientboundDatapacks : Clientbound
    {
        private const int Id = 0x0E;
        public override Client.EnumState Mode => Client.EnumState.Configuration;
        public ClientboundDatapacks((string @namespace, string id, string version)[] packs)
        {
            using MemoryStream stream = new();
            stream.WriteS32V(Id);
            stream.WriteS32V(packs.Length);
            for (int i = 0; i < packs.Length; i++)
            {
                (string @namespace, string id, string version) pack = packs[i];
                stream.WriteString(pack.@namespace, SizePrefix.S32V);
                stream.WriteString(pack.id, SizePrefix.S32V);
                stream.WriteString(pack.version, SizePrefix.S32V);
            }
            Buffer = stream.ToArray();
        }
        //TDOO: other registries
        internal override void Transform(Client client)
        {

        }
    }
}
