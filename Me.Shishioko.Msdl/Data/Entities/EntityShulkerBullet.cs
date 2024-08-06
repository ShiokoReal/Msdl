﻿namespace Me.Shishioko.Msdl.Data.Entities
{
    public sealed class EntityShulkerBullet : EntityBase
    {
        internal override int Id => 89;
        public override double HitboxHeight => 0.3125;
        public override double HitboxWidth => 0.3125;
        public override bool HitboxSoftCollision => false;
        public override bool HitboxHardCollision => false;
        public override bool HitboxAlign => false;
        public EntityShulkerBullet()
        {

        }
    }
}
