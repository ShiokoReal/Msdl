using Me.Shishioko.Msdl.Data.Blocks;

namespace Me.Shishioko.Msdl.Data.Items
{
    public sealed class ItemEnderEye : Item
    {
        internal override int Id => 1006;
        public override Block? Block => null;
        public ItemEnderEye()
        {

        }
        public override ItemEnderEye Clone()
        {
            return new();
        }
    }
}
