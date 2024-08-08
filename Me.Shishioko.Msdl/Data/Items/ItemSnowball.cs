using Me.Shishioko.Msdl.Data.Blocks;

namespace Me.Shishioko.Msdl.Data.Items
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
