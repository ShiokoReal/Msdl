using Net.Myzuc.PurpleStainedGlass.Protocol.Blocks;
using Net.Myzuc.PurpleStainedGlass.Protocol.Items;

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