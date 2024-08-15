using Net.Myzuc.PurpleStainedGlass.Protocol.Blocks;

namespace Net.Myzuc.PurpleStainedGlass.Protocol.Items
{
    public sealed class ItemExperienceBottle : Item
    {
        internal override int Id => 1088;
        public override Block? Block => null;
        public ItemExperienceBottle()
        {

        }
        public override ItemExperienceBottle Clone()
        {
            return new();
        }
    }
}
