namespace Net.Myzuc.PurpleStainedGlass.Protocol.Entities
{
    public sealed class EntityShulkerBullet : Entity
    {
        internal override int Id => 89;
        public override double HitboxHeight => 0.3125;
        public override double HitboxWidth => 0.3125;
        public override bool HitboxSoftCollision => false;
        public override bool HitboxHardCollision => false;
        public override bool HitboxAlign => false;
        public EntityShulkerBullet()
        {

        }
        public override EntityShulkerBullet Clone()
        {
            EntityShulkerBullet entity = new();
            entity.CloneFrom(this);
            return entity;
        }
    }
}
