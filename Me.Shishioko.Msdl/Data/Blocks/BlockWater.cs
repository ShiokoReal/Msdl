namespace Me.Shishioko.Msdl.Data.Blocks
{
    public sealed class BlockWater : BlockLiquid
    {
        internal override int BaseId => 80;
        public override byte LightEmit => 0;
        public override byte LightFilter => 1;
        public BlockWater()
        {

        }
        public override BlockWater Clone() => new()
        {
            Level = (byte)Level,
        };
    }
}
