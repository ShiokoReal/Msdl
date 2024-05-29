using System.Diagnostics.Contracts;

namespace Me.Shishioko.Msdl.Data
{
    public readonly struct Dimension
    {
        public readonly string Name;
        public readonly int Height;
        public readonly int Depth;
        public readonly Sky Effects { get; init; } = Sky.Overworld;
        public readonly bool Natural { get; init; } = true; //TODO: test without init
        public readonly bool HasSkylight { get; init; } = true;
        public readonly float AmbientLight { get; init; } = 0.0f;
        public Dimension(string name = "minecraft:overworld", int height = 256, int depth = 0)
        {
            Contract.Requires(!Connection.NamespaceRegex().IsMatch(name));
            Contract.Requires(depth + height < sbyte.MaxValue * 16);
            Contract.Requires(depth >= sbyte.MinValue * 16);
            Name = name;
            Height = height;
            Depth = depth;
        }
    }
}
