namespace Me.Shishioko.Msdl.Data.Entities
{
    public sealed class EntityExperience : Entity
    {
        internal override int Id => 38;
        public override double HitboxHeight => 0.5;
        public override double HitboxWidth => 0.5;
        public override bool HitboxSoftCollision => false;
        public override bool HitboxHardCollision => false;
        public override bool HitboxAlign => false;
        public EntityExperience()
        {

        }
    }
}
