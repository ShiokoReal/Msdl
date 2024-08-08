using Me.Shishioko.Msdl.Data.Blocks;

namespace Me.Shishioko.Msdl.Data.Items
{
    public sealed class ItemCobblestone : Item
    {
        internal override int Id => 35;
        public override BlockCobblestone? Block => new();
        public ItemCobblestone()
        {

        }
        public override ItemCobblestone Clone()
        {
            return new();
        }
    }
}
