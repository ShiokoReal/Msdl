using Net.Myzuc.ShioLib;
using System.IO;

namespace Net.Myzuc.PurpleStainedGlass.Protocol.Packets.Configuration
{
    public sealed class ClientboundFeatures : Clientbound
    {
        private const int Id = 0x0C;
        public static readonly string[] Features = ["minecraft:vanilla", "minecraft:bundle", "minecraft:trade_rebalance", "minecraft:update_1_21"];
        public override Client.EnumState Mode => Client.EnumState.Configuration;
        public ClientboundFeatures(string[] features)
        {
            using MemoryStream stream = new();
            stream.WriteS32V(Id);
            stream.WriteS32V(features.Length);
            foreach(string feature in features) stream.WriteString(feature, SizePrefix.S32V);
            Buffer = stream.ToArray();
        }
        internal override void Transform(Client client)
        {

        }
    }
}
