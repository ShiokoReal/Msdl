namespace Me.Shishioko.Msdl.Data.Blocks
{
    public abstract class BlockLiquid : Block
    {
        internal abstract int BaseId { get; }
        public sealed override int Id => BaseId + (InternalLevel & 15);
        public sealed override bool SoftHitboxCollision => false;
        public sealed override bool HardHitboxCollision => false;
        private byte InternalLevel = 8;
        public int Level
        {
            get => InternalLevel & 15;
            set => InternalLevel = (byte)(value & 15);
        }
        internal BlockLiquid()
        {

        }
    }
}
