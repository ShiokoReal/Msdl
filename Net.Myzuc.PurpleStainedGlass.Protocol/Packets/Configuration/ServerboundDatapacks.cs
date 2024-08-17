using Net.Myzuc.PurpleStainedGlass.Protocol.Enums;
using Net.Myzuc.ShioLib;
using System;
using System.IO;

namespace Net.Myzuc.PurpleStainedGlass.Protocol.Packets.Configuration
{
    public sealed class ServerboundDatapacks : Serverbound
    {
        public readonly (string @namespace, string id, string version)[] Packs;
        internal ServerboundDatapacks(MemoryStream stream)
        {
            Packs = new (string @namespace, string id, string version)[stream.ReadS32V()];
            for (int i = 0; i < Packs.Length; i++)
            {
                Packs[i] = (stream.ReadString(SizePrefix.S32V), stream.ReadString(SizePrefix.S32V), stream.ReadString(SizePrefix.S32V));
            }
        }
    }
}

