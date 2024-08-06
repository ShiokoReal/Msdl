namespace Me.Shishioko.Msdl.Data.Entities
{
    public sealed class EntityPearl : EntityItemProjectile
    {
        internal override int Id => 32;
        public override double HitboxHeight => 0.25;
        public override double HitboxWidth => 0.25;
        public override bool HitboxSoftCollision => false;
        public override bool HitboxHardCollision => false;
        public override bool HitboxAlign => false;
        public EntityPearl() : base(993) //TODO: read pearl id from item list
        {

        }
    }
}
