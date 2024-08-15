using Net.Myzuc.PurpleStainedGlass.Protocol.Blocks;

namespace Net.Myzuc.PurpleStainedGlass.Protocol.Items
{
    public sealed class ItemSnowball : Item
    {
        internal override int Id => 912;
        public override Block? Block => null;
        public ItemSnowball()
        {

        }
        public override ItemSnowball Clone()
        {
            return new();
        }
    }
}
