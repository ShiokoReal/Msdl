using Me.Shishioko.Msdl.Data.Blocks;

namespace Me.Shishioko.Msdl.Data.Items
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
