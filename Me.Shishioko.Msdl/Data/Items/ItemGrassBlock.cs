using Me.Shishioko.Msdl.Data.Blocks;
using Me.Shishioko.Msdl.Data.Items;

public sealed class ItemGrassBlock : Item
{
    internal override int Id => 27;
    public override BlockGrassBlock? Block => new();
    public ItemGrassBlock()
    {

    }
    public override ItemGrassBlock Clone()
    {
        return new();
    }
}