using Net.Myzuc.ShioLib;
using System.IO;

namespace Me.Shishioko.Msdl.Data
{
    public sealed class Item
    {
        public int Count;
        public int ID;
        public Item()
        {

        }
        internal void Serialize(Stream stream)
        {
            stream.WriteS32V(Count);
            if (Count <= 0) return;
            stream.WriteS32V(ID);
            stream.WriteS32V(0);
            stream.WriteS32V(0);
        }
    }
}
