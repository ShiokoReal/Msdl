using Me.Shishioko.Msdl.Data.Items;

namespace Me.Shishioko.Msdl.Data.Entities
{
    public sealed class EntityEnderPearl : EntityItemProjectile
    {
        internal override int Id => 32;
        public override double HitboxHeight => 0.25;
        public override double HitboxWidth => 0.25;
        public override bool HitboxSoftCollision => false;
        public override bool HitboxHardCollision => false;
        public override bool HitboxAlign => false;
        public EntityEnderPearl() : base(new ItemEnderPearl())
        {

        }
        public override EntityEnderPearl Clone()
        {
            EntityEnderPearl entity = new();
            entity.CloneFrom(this);
            return entity;
        }
    }
}
