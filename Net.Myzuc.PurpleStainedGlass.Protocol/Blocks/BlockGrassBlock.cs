namespace Net.Myzuc.PurpleStainedGlass.Protocol.Blocks
{
    public sealed class BlockGrassBlock : Block
    {
        public override int Id => Snowy ? 8 : 9;
        public sealed override byte LightEmit => 0;
        public sealed override byte LightFilter => 15;
        public sealed override bool SoftHitboxCollision => true;
        public sealed override bool HardHitboxCollision => true;
        public bool Snowy = false;
        public BlockGrassBlock()
        {

        }
        public override BlockGrassBlock Clone() => new()
        {
            Snowy = Snowy,
        };
    }
}
