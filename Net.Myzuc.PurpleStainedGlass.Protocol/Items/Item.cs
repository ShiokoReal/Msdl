using Net.Myzuc.PurpleStainedGlass.Protocol.Blocks;
using Net.Myzuc.ShioLib;
using System.IO;

namespace Net.Myzuc.PurpleStainedGlass.Protocol.Items
{
    public abstract class Item
    {
        internal abstract int Id { get; }
        public abstract Block? Block { get; }
        public int Count = 1;
        internal Item()
        {

        }
        public abstract Item Clone();
        internal void Serialize(Stream stream)
        {
            stream.WriteS32V(Count);
            if (Count <= 0) return;
            stream.WriteS32V(Id);
            stream.WriteS32V(0);
            stream.WriteS32V(0);
        }
    }
}
