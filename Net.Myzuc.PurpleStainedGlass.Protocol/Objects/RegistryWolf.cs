namespace Net.Myzuc.PurpleStainedGlass.Protocol.Objects
{
    public sealed class RegistryWolf
    {
        public string Name { get; }
        public string TextureWild { get; }
        public string TextureTame { get; }
        public string TextureAngry { get; }
        public RegistryWolf(string name, string textureWild, string textureTame, string textureAngry)
        {
            Name = name;
            TextureWild = textureWild;
            TextureTame = textureTame;
            TextureAngry = textureAngry;
        }
    }
}
