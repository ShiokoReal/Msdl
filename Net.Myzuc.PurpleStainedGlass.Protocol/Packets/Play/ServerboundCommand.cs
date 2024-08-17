using Net.Myzuc.ShioLib;
using System.IO;

namespace Net.Myzuc.PurpleStainedGlass.Protocol.Packets.Play
{
    public sealed class ServerboundChatCommand: Serverbound
    {
        public readonly string Content;
        internal ServerboundChatCommand(MemoryStream stream)
        {
            Content = stream.ReadString(SizePrefix.S32V, 32767);
        }
    }
}
