using System.Diagnostics.Contracts;

namespace Me.Shishioko.Msdl.Data
{
    public readonly struct Dimension
    {
        public readonly string Name;
        public readonly int Height;
        public readonly int Depth;
        public readonly Sky Effects { get; init; } = Sky.Overworld;
        public readonly bool Natural { get; init; } = true;
        public readonly bool HasSkylight { get; init; } = true;
        public readonly float AmbientLight { get; init; } = 0.0f;
        public Dimension(string name = "minecraft:overworld", int height = 256, int depth = 0)
        {
            //Contract.Assert(!Connection.NamespaceRegex().IsMatch(name)); //TODO: fix
            Contract.Assert(depth + height < sbyte.MaxValue * 16); //TODO: limit depth and height to what position supports or remove all checks
            Contract.Assert(depth >= sbyte.MinValue * 16);
            Name = name;
            Height = height;
            Depth = depth;
        }
    }
}
