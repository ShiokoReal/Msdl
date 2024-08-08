using Me.Shishioko.Msdl.Data.Blocks;

namespace Me.Shishioko.Msdl.Data.Items
{
    public sealed class ItemRedWool : Item
    {
        internal override int Id => 216;
        public override BlockRedWool? Block => new();
        public ItemRedWool()
        {

        }
        public override ItemRedWool Clone()
        {
            return new();
        }
    }
}
