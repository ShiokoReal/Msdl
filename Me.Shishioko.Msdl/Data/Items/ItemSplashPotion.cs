using Me.Shishioko.Msdl.Data.Blocks;

namespace Me.Shishioko.Msdl.Data.Items
{
    public sealed class ItemSplashPotion : Item
    {
        internal override int Id => 1158;
        public override Block? Block => null;
        public ItemSplashPotion()
        {

        }
        public override ItemSplashPotion Clone()
        {
            return new();
        }
    }
}
