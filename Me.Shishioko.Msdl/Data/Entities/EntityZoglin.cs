namespace Me.Shishioko.Msdl.Data.Entities
{
    public sealed class EntityZoglin : EntityGrowable
    {
        internal override int Id => 123;
        public override double HitboxHeight => 1.4;
        public override double HitboxWidth => 1.3964844;
        public override bool HitboxSoftCollision => true;
        public override bool HitboxHardCollision => false;
        public override bool HitboxAlign => false;
        public EntityZoglin()
        {

        }
        public override EntityZoglin Clone()
        {
            EntityZoglin entity = new();
            entity.CloneFrom(this);
            return entity;
        }
    }
}
