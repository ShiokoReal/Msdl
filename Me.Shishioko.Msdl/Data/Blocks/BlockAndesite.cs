namespace Me.Shishioko.Msdl.Data.Blocks
{
    public sealed class BlockAndesite : Block
    {
        public override int Id => 6;
        public sealed override byte LightEmit => 0;
        public sealed override byte LightFilter => 15;
        public sealed override bool SoftHitboxCollision => true;
        public sealed override bool HardHitboxCollision => true;
        public BlockAndesite()
        {

        }
        public override BlockAndesite Clone() => new();
    }
}
