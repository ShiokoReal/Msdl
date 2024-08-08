namespace Me.Shishioko.Msdl.Data.Entities
{
    public sealed class EntityWindCharge : Entity
    { //TODO: check if can be spawned and why not if not also check properties defined of collision
        internal override int Id => 117;
        public override double HitboxHeight => 0.3125;
        public override double HitboxWidth => 0.3125;
        public override bool HitboxSoftCollision => false;
        public override bool HitboxHardCollision => false;
        public override bool HitboxAlign => false;
        public EntityWindCharge()
        {

        }
    }
}
