namespace Net.Myzuc.PurpleStainedGlass.Protocol.Objects
{
    public sealed class RegistryPainting
    {
        public string Name { get; }
        public string Texture { get; }
        public int Width { get; }
        public int Height { get; }
        public RegistryPainting(string name, string texture, int width, int height)
        {
            Name = name;
            Texture = texture;
            Width = width;
            Height = height;
        }
    }
}
