using System.IO;
using Me.Shishioko.Msdl.Data.Protocol;
using Net.Myzuc.ShioLib;

namespace Me.Shishioko.Msdl.Data.Entities
{
    public sealed class EntityWarden : EntityMob
    {
        internal override int Id => 116;
        public override double HitboxHeight => 2.9;
        public override double HitboxWidth => 0.9;
        public override bool HitboxSoftCollision => true;
        public override bool HitboxHardCollision => false;
        public override bool HitboxAlign => false;
        public int Anger = 0;
        public EntityWarden()
        {

        }
        internal override void Serialize(Stream stream, Entity? rawDifference)
        {
            base.Serialize(stream, rawDifference);
            EntityWarden? difference = rawDifference is EntityWarden castDifference ? castDifference : null;
            if (difference is not null ? difference.Anger != Anger : true)
            {
                stream.WriteU8(16);
                stream.WriteU8(MetadataType.S32V);
                stream.WriteS32V(Anger);
            }
        }
        public override void Clone(Entity rawEntity)
        {
            base.Clone(rawEntity);
            if (rawEntity is not EntityWarden entity) return;
            Anger = entity.Anger;
        }
    }
}
