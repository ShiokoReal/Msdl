namespace Net.Myzuc.PurpleStainedGlass.Protocol.Blocks
{
    public abstract class BlockAir : BlockFluid
    {
        public override int FluidId => 0;
        public override byte LightEmit => 0;
        public override byte LightFilter => 0;
        internal BlockAir()
        {

        }
        public abstract override BlockAir Clone();
    }
}
