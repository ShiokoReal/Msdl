namespace Net.Myzuc.PurpleStainedGlass.Protocol.Entities
{
    public sealed class EntityMarker : Entity
    {
        internal override int Id => 68;
        public override double HitboxHeight => 0.0;
        public override double HitboxWidth => 0.0;
        public override bool HitboxSoftCollision => false;
        public override bool HitboxHardCollision => false;
        public override bool HitboxAlign => false;
        public EntityMarker()
        {

        }
        public override EntityMarker Clone()
        {
            EntityMarker entity = new();
            entity.CloneFrom(this);
            return entity;
        }
    }
}
