using Net.Myzuc.PurpleStainedGlass.Protocol.Blocks;

namespace Net.Myzuc.PurpleStainedGlass.Protocol.Items
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
