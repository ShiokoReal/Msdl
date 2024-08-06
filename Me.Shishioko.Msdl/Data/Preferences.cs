using Me.Shishioko.Msdl.Data.Entities;

namespace Me.Shishioko.Msdl.Data
{
    public sealed class Preferences
    {
        public readonly string Language;
        public readonly byte RenderDistance;
        public readonly ChatMode ChatMode;
        public readonly bool ChatColors;
        public readonly EntityPlayer.EntityPlayerSkinMask SkinMask;
        public readonly bool RightHanded;
        public readonly bool Listing;
        internal Preferences(string language, byte renderDistance, ChatMode chatMode, bool chatColors, EntityPlayer.EntityPlayerSkinMask skinMask, bool rightHanded, bool listing)
        {
            Language = language;
            RenderDistance = renderDistance;
            ChatMode = chatMode;
            ChatColors = chatColors;
            SkinMask = skinMask;
            RightHanded = rightHanded;
            Listing = listing;
        }
    }
}
