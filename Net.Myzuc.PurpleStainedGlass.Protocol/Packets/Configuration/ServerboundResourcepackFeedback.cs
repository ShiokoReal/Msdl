using Net.Myzuc.PurpleStainedGlass.Protocol.Enums;
using Net.Myzuc.ShioLib;
using System;
using System.IO;

namespace Net.Myzuc.PurpleStainedGlass.Protocol.Packets.Configuration
{
    public sealed class ServerboundResourcepackFeedback : Serverbound
    {
        public readonly Guid Guid;
        public readonly EnumDownloadFeedback Feedback;
        internal ServerboundResourcepackFeedback(MemoryStream stream)
        {
            Guid = stream.ReadGuid();
            Feedback = (EnumDownloadFeedback)stream.ReadS32V();
        }
    }
}

