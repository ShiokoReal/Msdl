namespace Net.Myzuc.PurpleStainedGlass.Protocol.Objects
{
    public sealed class RegistryChat
    {
        public string Name { get; }
        public Styling ChatTranslationStyling { get; init; }
        public Translation ChatTranslation { get; }
        public Translation NarratorTranslation { get; }
        public RegistryChat(string name, Translation chat, Translation narrator)
        {
            Name = name;
            ChatTranslation = chat;
            NarratorTranslation = narrator;
        }
    }
}
