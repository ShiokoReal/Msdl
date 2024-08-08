using Me.Shishioko.Msdl.Data.Blocks;

namespace Me.Shishioko.Msdl.Data.Items
{
    public sealed class ItemWaterBucket : Item
    {
        internal override int Id => 909;
        public override BlockWater Block => new();
        public ItemWaterBucket()
        {

        }
        public override ItemWaterBucket Clone()
        {
            return new();
        }
    }
}
