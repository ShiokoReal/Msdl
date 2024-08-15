namespace Net.Myzuc.PurpleStainedGlass.Protocol.Blocks
{
    public sealed class BlockFire : Block
    {
        public override int Id => 2360;
        public sealed override byte LightEmit => 15;
        public sealed override byte LightFilter => 0;
        public sealed override bool SoftHitboxCollision => true;
        public sealed override bool HardHitboxCollision => true;
        //TODO: states
        public BlockFire()
        {

        }
        public override BlockFire Clone() => new(); //TODO: clone state
    }
}
