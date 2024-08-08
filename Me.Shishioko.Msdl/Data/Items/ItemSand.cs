using Me.Shishioko.Msdl.Data.Blocks;

namespace Me.Shishioko.Msdl.Data.Items
{
    public sealed class ItemSand : Item
    {
        internal override int Id => 57;
        public override BlockSand? Block => new();
        public ItemSand()
        {

        }
        public override ItemSand Clone()
        {
            return new();
        }
    }
}
