namespace Net.Myzuc.PurpleStainedGlass.Protocol.Objects
{
    public sealed class RegistryBanner
    {
        public string Name { get; }
        public string Texture { get; }
        public string TranslationKey { get; }
        public RegistryBanner(string name, string texture, string translationKey)
        {
            Name = name;
            Texture = texture;
            TranslationKey = translationKey;
        }
    }
}
