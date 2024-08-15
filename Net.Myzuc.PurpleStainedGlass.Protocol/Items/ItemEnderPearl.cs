using Net.Myzuc.PurpleStainedGlass.Protocol.Blocks;

namespace Net.Myzuc.PurpleStainedGlass.Protocol.Items
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
