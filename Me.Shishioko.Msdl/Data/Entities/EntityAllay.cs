using System.IO;
using Me.Shishioko.Msdl.Data.Protocol;
using Net.Myzuc.ShioLib;

namespace Me.Shishioko.Msdl.Data.Entities
{
    public sealed class EntityAllay : EntityMob
    {
        internal override int Id => 0;
        public override double HitboxHeight => 0.6;
        public override double HitboxWidth => 0.35;
        public override bool HitboxSoftCollision => true;
        public override bool HitboxHardCollision => false;
        public override bool HitboxAlign => false;
        public bool Dancing = false;
        public bool Breedable = false;
        public EntityAllay()
        {

        }
        internal override void Serialize(Stream stream, Entity? rawDifference)
        {
            base.Serialize(stream, rawDifference);
            EntityAllay? difference = rawDifference is EntityAllay castDifference ? castDifference : null;
            if (difference is not null ? difference.Dancing != Dancing : true)
            {
                stream.WriteU8(16);
                stream.WriteU8(MetadataType.Bool);
                stream.WriteBool(Dancing);
            }
            if (difference is not null ? difference.Breedable != Breedable : true)
            {
                stream.WriteU8(17);
                stream.WriteU8(MetadataType.Bool);
                stream.WriteBool(Breedable);
            }
        }
        public override void CloneFrom(Entity rawEntity)
        {
            base.CloneFrom(rawEntity);
            if (rawEntity is not EntityAllay entity) return;
            Dancing = entity.Dancing;
            Breedable = entity.Breedable;
        }
        public override EntityAllay Clone()
        {
            EntityAllay entity = new();
            entity.CloneFrom(this);
            return entity;
        }
    }
}
