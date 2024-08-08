namespace Me.Shishioko.Msdl.Data.Blocks
{
    public sealed class BlockDiorite : Block
    {
        public override int Id => 4;
        public sealed override byte LightEmit => 0;
        public sealed override byte LightFilter => 15;
        public sealed override bool SoftHitboxCollision => true;
        public sealed override bool HardHitboxCollision => true;
        public BlockDiorite()
        {

        }
        public override BlockDiorite Clone() => new();
    }
}
