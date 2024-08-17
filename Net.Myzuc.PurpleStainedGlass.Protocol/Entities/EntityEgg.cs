using Net.Myzuc.PurpleStainedGlass.Protocol.Items;

namespace Net.Myzuc.PurpleStainedGlass.Protocol.Entities
{
    public sealed class EntityEgg : EntityItemProjectile
    {
        internal override int Id => 28;
        public override double HitboxHeight => 0.25;
        public override double HitboxWidth => 0.25;
        public override bool HitboxSoftCollision => false;
        public override bool HitboxHardCollision => false;
        public override bool HitboxAlign => false;
        public EntityEgg() : base(new ItemEgg())
        {

        }
        public override EntityEgg Clone()
        {
            EntityEgg entity = new();
            entity.CloneFrom(this);
            return entity;
        }
    }
}
