using System.Diagnostics.Contracts;
using System.Xml.Linq;

namespace Me.Shishioko.Msdl.Data
{
    public readonly struct DeathLocation
    {
        public readonly string Dimension;
        public readonly Position Location;
        public DeathLocation(string dimension, Position location)
        {
            Dimension = dimension;
            Location = location;
        }
    }
}
