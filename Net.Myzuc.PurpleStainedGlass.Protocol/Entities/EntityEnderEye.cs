using Net.Myzuc.PurpleStainedGlass.Protocol.Items;

namespace Net.Myzuc.PurpleStainedGlass.Protocol.Entities
{
    public sealed class EntityEnderEye : EntityItemProjectile
    {
        internal override int Id => 39;
        public override double HitboxHeight => 0.25;
        public override double HitboxWidth => 0.25;
        public override bool HitboxSoftCollision => false;
        public override bool HitboxHardCollision => false;
        public override bool HitboxAlign => false;
        public EntityEnderEye() : base(new ItemEnderEye())
        {

        }
        public override EntityEnderEye Clone()
        {
            EntityEnderEye entity = new();
            entity.CloneFrom(this);
            return entity;
        }
    }
}
