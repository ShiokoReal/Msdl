namespace Net.Myzuc.PurpleStainedGlass.Protocol.Entities
{
    public sealed class EntityLlamaSpit : Entity
    {
        internal override int Id => 66;
        public override double HitboxHeight => 0.25;
        public override double HitboxWidth => 0.25;
        public override bool HitboxSoftCollision => false;
        public override bool HitboxHardCollision => false;
        public override bool HitboxAlign => false;
        public EntityLlamaSpit()
        {

        }
        public override EntityLlamaSpit Clone()
        {
            EntityLlamaSpit entity = new();
            entity.CloneFrom(this);
            return entity;
        }
    }
}
