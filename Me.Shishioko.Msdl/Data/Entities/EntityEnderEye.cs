namespace Me.Shishioko.Msdl.Data.Entities
{
    public sealed class EntityEnderEye : EntityItemProjectile
    {
        internal override int Id => 39;
        public override double HitboxHeight => 0.25;
        public override double HitboxWidth => 0.25;
        public override bool HitboxSoftCollision => false;
        public override bool HitboxHardCollision => false;
        public override bool HitboxAlign => false;
        public EntityEnderEye() : base(1006) //TODO: read eye id from item list
        {

        }
    }
}
