using Me.Shishioko.Msdl.Data.Blocks;

namespace Me.Shishioko.Msdl.Data.Items
{
    public sealed class ItemEnderPearl : Item
    {
        internal override int Id => 993;
        public override Block? Block => null;
        public ItemEnderPearl()
        {

        }
        public override ItemEnderPearl Clone()
        {
            return new();
        }
    }
}
