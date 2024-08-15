namespace Net.Myzuc.PurpleStainedGlass.Protocol.Entities
{
    public sealed class EntityPhantom : EntityResizable
    {
        internal override int Id => 76;
        public override double HitboxHeight => 0.5 + 0.1 * Size;
        public override double HitboxWidth => 0.9 + 0.2 * Size;
        public override bool HitboxSoftCollision => true;
        public override bool HitboxHardCollision => false;
        public override bool HitboxAlign => false;
        public EntityPhantom()
        {

        }
        public override EntityPhantom Clone()
        {
            EntityPhantom entity = new();
            entity.CloneFrom(this);
            return entity;
        }
    }
}
