namespace Net.Myzuc.PurpleStainedGlass.Protocol.Objects
{
    public sealed class Translation
    {
        public string Key { get; }
        private readonly string[]? InternalArguments;
        public string[]? Arguments => InternalArguments is not null ? [.. InternalArguments] : null;
        public Translation(string translate, string[]? arguments = null)
        {
            Key = translate;
            InternalArguments = arguments is null ? null : [.. arguments];
        }
    }
}
