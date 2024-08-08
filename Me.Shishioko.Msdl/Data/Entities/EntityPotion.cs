using Me.Shishioko.Msdl.Data.Items;

namespace Me.Shishioko.Msdl.Data.Entities
{
    public sealed class EntityPotion : EntityItemProjectile
    {
        internal override int Id => 82;
        public override double HitboxHeight => 0.25;
        public override double HitboxWidth => 0.25;
        public override bool HitboxSoftCollision => false;
        public override bool HitboxHardCollision => false;
        public override bool HitboxAlign => false;
        public EntityPotion() : base(new ItemSplashPotion())
        {

        }
        public override EntityPotion Clone()
        {
            EntityPotion entity = new();
            entity.CloneFrom(this);
            return entity;
        }
    }
}
