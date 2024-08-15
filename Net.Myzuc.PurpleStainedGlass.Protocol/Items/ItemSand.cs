using Net.Myzuc.PurpleStainedGlass.Protocol.Blocks;

namespace Net.Myzuc.PurpleStainedGlass.Protocol.Items
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
