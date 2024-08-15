using Net.Myzuc.PurpleStainedGlass.Protocol.Blocks;

namespace Net.Myzuc.PurpleStainedGlass.Protocol.Items
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
