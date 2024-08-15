namespace Net.Myzuc.PurpleStainedGlass.Protocol.Blocks
{
    public sealed class BlockCrimsonPlanks : Block
    {
        public override int Id => 18666;
        public sealed override byte LightEmit => 0;
        public sealed override byte LightFilter => 15;
        public sealed override bool SoftHitboxCollision => true;
        public sealed override bool HardHitboxCollision => true;
        public BlockCrimsonPlanks()
        {

        }
        public override BlockCrimsonPlanks Clone() => new();
    }
}
