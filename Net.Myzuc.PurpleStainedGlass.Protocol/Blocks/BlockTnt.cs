namespace Net.Myzuc.PurpleStainedGlass.Protocol.Blocks
{
    public sealed class BlockTnt : Block
    {
        public override int Id => Unstable ? 2094 : 2095;
        public sealed override byte LightEmit => 0;
        public sealed override byte LightFilter => 15;
        public sealed override bool SoftHitboxCollision => true;
        public sealed override bool HardHitboxCollision => true;
        public bool Unstable = false;
        public BlockTnt()
        {

        }
        public override BlockTnt Clone() => new()
        {
            Unstable = Unstable
        };
    }
}
