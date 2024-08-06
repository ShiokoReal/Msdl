namespace Me.Shishioko.Msdl.Data.Entities
{
    public sealed class EntityLlamaSpit : EntityBase
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
    }
}
