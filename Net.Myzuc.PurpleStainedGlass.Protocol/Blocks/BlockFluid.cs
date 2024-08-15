namespace Net.Myzuc.PurpleStainedGlass.Protocol.Blocks
{
    public abstract class BlockFluid : Block
    {
        public abstract int FluidId { get; }
        public sealed override bool SoftHitboxCollision => false;
        public sealed override bool HardHitboxCollision => false;
        internal BlockFluid()
        {

        }
        public abstract override BlockFluid Clone();
    }
}
