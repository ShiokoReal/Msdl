namespace Me.Shishioko.Msdl.Data.Entities
{
    public sealed class EntityFireball : EntityItemProjectile
    {
        internal override int Id => 62;
        public override double HitboxHeight => 1;
        public override double HitboxWidth => 1;
        public override bool HitboxSoftCollision => false;
        public override bool HitboxHardCollision => false;
        public override bool HitboxAlign => false;
        public EntityFireball() : base(1089) //TODO: read firecharge id from item list
        {

        }
    }
}
