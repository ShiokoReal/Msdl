using System.Collections.Generic;

namespace Net.Myzuc.PurpleStainedGlass.Generator
{
    public sealed class Property
    {
        public readonly string Name;
        public readonly IReadOnlyList<string>? Values;
        public readonly int? Range;
        public Property(string name, IReadOnlyList<string>? values = null)
        {
            Name = name;
            Values = values;
            Range = null;
        }
        public Property(string name, int range)
        {
            Name = name;
            Values = null;
            Range = range;
        }
    }
}
