namespace Me.Shishioko.Msdl.Data.Blocks
{
    public sealed class BlockSand : Block
    {
        public override int Id => 112;
        public sealed override byte LightEmit => 0;
        public sealed override byte LightFilter => 15;
        public sealed override bool SoftHitboxCollision => true;
        public sealed override bool HardHitboxCollision => true;
        public BlockSand()
        {

        }
        public override BlockSand Clone() => new();
    }
}
