namespace Net.Myzuc.PurpleStainedGlass.Protocol.Blocks
{
    public sealed class BlockCobblestone : Block
    {
        public override int Id => 14;
        public sealed override byte LightEmit => 0;
        public sealed override byte LightFilter => 15;
        public sealed override bool SoftHitboxCollision => true;
        public sealed override bool HardHitboxCollision => true;
        public BlockCobblestone()
        {

        }
        public override BlockCobblestone Clone() => new();
    }
}
