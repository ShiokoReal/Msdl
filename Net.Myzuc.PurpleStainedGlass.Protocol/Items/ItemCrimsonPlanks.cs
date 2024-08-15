using Net.Myzuc.PurpleStainedGlass.Protocol.Blocks;
using Net.Myzuc.PurpleStainedGlass.Protocol.Items;

public sealed class ItemCrimsonPlanks : Item
{
    internal override int Id => 45;
    public override BlockCrimsonPlanks? Block => new();
    public ItemCrimsonPlanks()
    {

    }
    public override ItemCrimsonPlanks Clone()
    {
        return new();
    }
}