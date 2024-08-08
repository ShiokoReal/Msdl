namespace Me.Shishioko.Msdl.Data.Blocks
{
    public sealed class BlockGranite : Block
    {
        public override int Id => 2;
        public sealed override byte LightEmit => 0;
        public sealed override byte LightFilter => 15;
        public sealed override bool SoftHitboxCollision => true;
        public sealed override bool HardHitboxCollision => true;
        public BlockGranite()
        {

        }
        public override BlockGranite Clone() => new();
    }
}
