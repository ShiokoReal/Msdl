using Net.Myzuc.PurpleStainedGlass.Protocol.Enums;
using Net.Myzuc.PurpleStainedGlass.Protocol.Objects;
using Net.Myzuc.ShioLib;
using System.IO;

namespace Net.Myzuc.PurpleStainedGlass.Protocol.Packets.Configuration
{
    public sealed class ServerboundPreferences : Serverbound
    {
        public readonly Preferences Preferences;
        internal ServerboundPreferences(MemoryStream stream)
        {
            string language = stream.ReadString(SizePrefix.S32V, 16);
            byte renderDistance = stream.ReadU8();
            EnumChatMode chatMode = (EnumChatMode)stream.ReadS32V();
            bool chatColors = stream.ReadBool();
            FlagsSkin skin = (FlagsSkin)stream.ReadU8();
            EnumHand mainHand = (EnumHand)stream.ReadS32V();
            bool textFiltering = stream.ReadBool();
            bool listing = stream.ReadBool();
            Preferences = new(language, renderDistance, chatMode, chatColors, skin, mainHand, textFiltering, listing);
        }
    }
}
