using Net.Myzuc.PurpleStainedGlass.Protocol.Data.Items;

namespace Net.Myzuc.PurpleStainedGlass.Protocol.Entities
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
