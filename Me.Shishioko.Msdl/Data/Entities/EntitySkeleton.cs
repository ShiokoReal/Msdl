using System.Drawing;
using System.IO;
using Me.Shishioko.Msdl.Data.Protocol;
using Net.Myzuc.ShioLib;

namespace Me.Shishioko.Msdl.Data.Entities
{
    public sealed class EntitySkeleton : EntityMob
    {
        internal override int Id => 91;
        public override double HitboxHeight => 1.99;
        public override double HitboxWidth => 0.6;
        public override bool HitboxSoftCollision => true;
        public override bool HitboxHardCollision => false;
        public override bool HitboxAlign => false;
        public bool Transforming = false;
        public EntitySkeleton()
        {

        }
        internal override void Serialize(Stream stream, Entity? rawDifference)
        {
            base.Serialize(stream, rawDifference);
            EntitySkeleton? difference = rawDifference is EntitySkeleton castDifference ? castDifference : null;
            if (difference is not null ? difference.Transforming != Transforming : true)
            {
                stream.WriteU8(16);
                stream.WriteU8(MetadataType.Bool);
                stream.WriteBool(Transforming);
            }
        }
        public override void Clone(Entity rawEntity)
        {
            base.Clone(rawEntity);
            if (rawEntity is not EntitySkeleton entity) return;
            Transforming = entity.Transforming;
        }
    }
}
