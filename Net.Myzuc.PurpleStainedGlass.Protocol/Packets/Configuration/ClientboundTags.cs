using Net.Myzuc.PurpleStainedGlass.Protocol.Blocks;
using Net.Myzuc.ShioLib;
using System.IO;

namespace Net.Myzuc.PurpleStainedGlass.Protocol.Packets.Configuration
{
    public sealed class ClientboundTags : Clientbound
    {
        private const int Id = 0x0D;
        public override Client.EnumState Mode => Client.EnumState.Configuration;
        public ClientboundTags(BlockLiquid[] water, BlockLiquid[] lava)
        {
            using MemoryStream stream = new();
            stream.WriteS32V(Id);
            stream.WriteS32V(1);
            stream.WriteString("minecraft:fluid", SizePrefix.S32V);
            stream.WriteS32V(2);
            stream.WriteString("water", SizePrefix.S32V);
            stream.WriteS32V(water.Length);
            for (int i = 0; i < water.Length; i++)
            {
                stream.WriteS32V(water[i].FluidId);
            }
            stream.WriteString("lava", SizePrefix.S32V);
            stream.WriteS32V(lava.Length);
            for (int i = 0; i < lava.Length; i++)
            {
                stream.WriteS32V(lava[i].FluidId);
            }
            Buffer = stream.ToArray();
        }
        //TDOO: other registries
        internal override void Transform(Client client)
        {

        }
    }
}
