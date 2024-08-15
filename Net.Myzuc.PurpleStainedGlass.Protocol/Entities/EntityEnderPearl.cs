using Net.Myzuc.PurpleStainedGlass.Protocol.Data.Items;

namespace Net.Myzuc.PurpleStainedGlass.Protocol.Entities
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
