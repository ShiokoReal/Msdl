namespace Me.Shishioko.Msdl.Data.Blocks
{
    public sealed class BlockStone : Block
    {
        public override int Id => 1;
        public sealed override byte LightEmit => 0;
        public sealed override byte LightFilter => 15;
        public sealed override bool SoftHitboxCollision => true;
        public sealed override bool HardHitboxCollision => true;
        public BlockStone()
        {

        }
        public override BlockStone Clone() => new();
    }
}
