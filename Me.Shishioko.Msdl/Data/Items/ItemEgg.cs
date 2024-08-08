using Me.Shishioko.Msdl.Data.Blocks;

namespace Me.Shishioko.Msdl.Data.Items
{
    public sealed class ItemEgg : Item
    {
        internal override int Id => 927;
        public override Block? Block => null;
        public ItemEgg()
        {

        }
        public override ItemEgg Clone()
        {
            return new();
        }
    }
}
