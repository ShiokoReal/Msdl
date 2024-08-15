namespace Net.Myzuc.PurpleStainedGlass.Protocol.Blocks
{
    public sealed class BlockWater : BlockLiquid
    {
        public override int FluidId => 1;
        internal override int BaseId => 80;
        public override byte LightEmit => 0;
        public override byte LightFilter => 1;
        public BlockWater()
        {

        }
        public override BlockWater Clone() => new()
        {
            Level = Level,
            Flowing = Flowing,
        };
    }
}
