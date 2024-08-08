namespace Me.Shishioko.Msdl.Data.Entities
{
    public sealed class EntityLightning : Entity
    {
        internal override int Id => 64;
        public override double HitboxHeight => 0.0;
        public override double HitboxWidth => 0.0;
        public override bool HitboxSoftCollision => false;
        public override bool HitboxHardCollision => false;
        public override bool HitboxAlign => false;
        public EntityLightning()
        {

        }
    }
}
