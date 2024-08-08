namespace Me.Shishioko.Msdl.Data.Blocks
{
    public sealed class BlockRedStainedGlass : Block
    {
        public sealed override int Id => 5959;
        public sealed override byte LightEmit => 0;
        public sealed override byte LightFilter => 0;
        public sealed override bool SoftHitboxCollision => true;
        public sealed override bool HardHitboxCollision => true;
        public BlockRedStainedGlass()
        {

        }
        public override BlockRedStainedGlass Clone() => new();
    }
}
