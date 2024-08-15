using Net.Myzuc.PurpleStainedGlass.Protocol.Blocks;

namespace Net.Myzuc.PurpleStainedGlass.Protocol.Items
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
