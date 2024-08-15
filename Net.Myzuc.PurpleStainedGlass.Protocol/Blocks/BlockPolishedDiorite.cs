namespace Net.Myzuc.PurpleStainedGlass.Protocol.Blocks
{
    public sealed class BlockPolishedDiorite : Block
    {
        public override int Id => 5;
        public sealed override byte LightEmit => 0;
        public sealed override byte LightFilter => 15;
        public sealed override bool SoftHitboxCollision => true;
        public sealed override bool HardHitboxCollision => true;
        public BlockPolishedDiorite()
        {

        }
        public override BlockPolishedDiorite Clone() => new();
    }
}
