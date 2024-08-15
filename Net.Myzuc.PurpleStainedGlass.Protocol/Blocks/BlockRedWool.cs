namespace Net.Myzuc.PurpleStainedGlass.Protocol.Blocks
{
    public sealed class BlockRedWool : Block
    {
        public sealed override int Id => 2061;
        public sealed override byte LightEmit => 0;
        public sealed override byte LightFilter => 15;
        public sealed override bool SoftHitboxCollision => true;
        public sealed override bool HardHitboxCollision => true;
        public BlockRedWool()
        {

        }
        public override BlockRedWool Clone() => new();
    }
}
