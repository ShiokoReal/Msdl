using Net.Myzuc.PurpleStainedGlass.Protocol.Enums;

namespace Net.Myzuc.PurpleStainedGlass.Protocol.Objects
{
    public sealed class Preferences
    {
        public string Language { get; }
        public byte RenderDistance { get; }
        public EnumChatMode ChatMode { get; }
        public bool ChatColors { get; }
        public FlagsSkin Skin { get; }
        public EnumHand MainHand { get; }
        public bool TextFiltering { get; }
        public bool Listing { get; }
        internal Preferences(string language, byte renderDistance, EnumChatMode chatMode, bool chatColors, FlagsSkin skin, EnumHand mainHand, bool textFiltering, bool listing)
        {
            Language = language;
            RenderDistance = renderDistance;
            ChatMode = chatMode;
            ChatColors = chatColors;
            Skin = skin;
            MainHand = mainHand;
            TextFiltering = textFiltering;
            Listing = listing;
        }
    }
}
