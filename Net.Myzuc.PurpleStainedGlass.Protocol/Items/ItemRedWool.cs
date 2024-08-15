using Net.Myzuc.PurpleStainedGlass.Protocol.Blocks;

namespace Net.Myzuc.PurpleStainedGlass.Protocol.Items
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
