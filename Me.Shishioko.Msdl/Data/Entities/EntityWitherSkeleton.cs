namespace Me.Shishioko.Msdl.Data.Entities
{
    public sealed class EntityWitherSkeleton : EntityMob
    {
        internal override int Id => 120;
        public override double HitboxHeight => 2.4;
        public override double HitboxWidth => 0.7;
        public override bool HitboxSoftCollision => true;
        public override bool HitboxHardCollision => false;
        public override bool HitboxAlign => false;
        public EntityWitherSkeleton()
        {

        }
        public override EntityWitherSkeleton Clone()
        {
            EntityWitherSkeleton entity = new();
            entity.CloneFrom(this);
            return entity;
        }
    }
}
