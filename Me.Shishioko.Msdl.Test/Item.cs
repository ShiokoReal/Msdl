namespace Me.Shishioko.Msdl.Test
{
    public sealed class Item
    {
        public readonly int ItemID;
        public readonly int BlockID;
        public Item(int itemID, int blockID)
        {
            ItemID = itemID;
            BlockID = blockID;
        }
    }
}
