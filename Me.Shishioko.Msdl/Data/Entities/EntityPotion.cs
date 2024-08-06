namespace Me.Shishioko.Msdl.Data.Entities
{
    public sealed class EntityPotion : EntityItemProjectile
    {
        internal override int Id => 82;
        public override double HitboxHeight => 0.25;
        public override double HitboxWidth => 0.25;
        public override bool HitboxSoftCollision => false;
        public override bool HitboxHardCollision => false;
        public override bool HitboxAlign => false;
        public EntityPotion() : base(1158) //TODO: read potion id from item list
        {

        }
    }
}
