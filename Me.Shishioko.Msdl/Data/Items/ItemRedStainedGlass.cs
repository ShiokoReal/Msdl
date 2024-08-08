using Me.Shishioko.Msdl.Data.Blocks;

namespace Me.Shishioko.Msdl.Data.Items
{
    public sealed class ItemRedStainedGlass : Item
    {
        internal override int Id => 485;
        public override BlockRedStainedGlass? Block => new();
        public ItemRedStainedGlass()
        {

        }
        public override ItemRedStainedGlass Clone()
        {
            return new();
        }
    }
}
