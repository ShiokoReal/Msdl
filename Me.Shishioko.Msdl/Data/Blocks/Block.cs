namespace Me.Shishioko.Msdl.Data.Blocks
{
    public abstract class Block
    {
        public abstract int Id { get; }
        public abstract byte LightEmit { get; }
        public abstract byte LightFilter { get; }
        public abstract bool SoftHitboxCollision { get; }
        public abstract bool HardHitboxCollision { get; }
        internal Block()
        {

        }
        public abstract Block Clone();
    }
}
