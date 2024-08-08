using Me.Shishioko.Msdl.Data.Blocks;
using Me.Shishioko.Msdl.Data.Items;

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