using Net.Myzuc.PurpleStainedGlass.Protocol.Blocks;

namespace Net.Myzuc.PurpleStainedGlass.Protocol.Items
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
