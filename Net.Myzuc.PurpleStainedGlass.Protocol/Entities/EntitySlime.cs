using System.IO;
using Net.Myzuc.PurpleStainedGlass.Protocol.Data.Protocol;
using Net.Myzuc.ShioLib;

namespace Net.Myzuc.PurpleStainedGlass.Protocol.Entities
{
    public sealed class EntitySlime : EntityResizable
    {
        internal override int Id => 93;
        public override double HitboxHeight => 0.5202 * Size;
        public override double HitboxWidth => 0.5202 * Size;
        public override bool HitboxSoftCollision => true;
        public override bool HitboxHardCollision => false;
        public override bool HitboxAlign => false;
        public EntitySlime()
        {
            Size = 1;
        }
        public override EntitySlime Clone()
        {
            EntitySlime entity = new();
            entity.CloneFrom(this);
            return entity;
        }
    }
}
