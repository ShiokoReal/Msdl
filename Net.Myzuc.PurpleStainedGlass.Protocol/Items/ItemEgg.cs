using Net.Myzuc.PurpleStainedGlass.Protocol.Blocks;

namespace Net.Myzuc.PurpleStainedGlass.Protocol.Items
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
