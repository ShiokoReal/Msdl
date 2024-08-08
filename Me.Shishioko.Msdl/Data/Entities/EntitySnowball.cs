using Me.Shishioko.Msdl.Data.Items;

namespace Me.Shishioko.Msdl.Data.Entities
{
    public sealed class EntitySnowball : EntityItemProjectile
    {
        internal override int Id => 97;
        public override double HitboxHeight => 0.25;
        public override double HitboxWidth => 0.25;
        public override bool HitboxSoftCollision => false;
        public override bool HitboxHardCollision => false;
        public override bool HitboxAlign => false;
        public EntitySnowball() : base(new ItemSnowball())
        {

        }
        public override EntitySnowball Clone()
        {
            EntitySnowball entity = new();
            entity.CloneFrom(this);
            return entity;
        }
    }
}
