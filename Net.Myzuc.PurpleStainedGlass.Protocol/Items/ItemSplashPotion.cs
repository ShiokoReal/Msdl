using Net.Myzuc.PurpleStainedGlass.Protocol.Blocks;

namespace Net.Myzuc.PurpleStainedGlass.Protocol.Items
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
