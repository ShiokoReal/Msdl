using Net.Myzuc.PurpleStainedGlass.Protocol.Blocks;

namespace Net.Myzuc.PurpleStainedGlass.Protocol.Items
{
    public sealed class ItemRedStainedGlass : Item
    {
        internal override int Id => 485;
        public override BlockRedStainedGlass? Block => new();
        public ItemRedStainedGlass()
        {

        }
        public override ItemRedStainedGlass Clone()
        {
            return new();
        }
    }
}
