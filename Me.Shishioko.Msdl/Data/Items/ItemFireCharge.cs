using Me.Shishioko.Msdl.Data.Blocks;

namespace Me.Shishioko.Msdl.Data.Items
{
    public sealed class ItemFireCharge : Item
    {
        internal override int Id => 1089;
        public override BlockFire? Block => new();
        public ItemFireCharge()
        {

        }
        public override ItemFireCharge Clone()
        {
            return new();
        }
    }
}
