using System.IO;
using Net.Myzuc.PurpleStainedGlass.Protocol.Enums;
using Net.Myzuc.PurpleStainedGlass.Protocol.Const;
using Net.Myzuc.ShioLib;

namespace Net.Myzuc.PurpleStainedGlass.Protocol.Entities
{
    public sealed class EntitySnowGolem : EntityMob
    {
        internal override int Id => 96;
        public override double HitboxHeight => 1.9;
        public override double HitboxWidth => 0.7;
        public override bool HitboxSoftCollision => true;
        public override bool HitboxHardCollision => false;
        public override bool HitboxAlign => false;
        public FlagsEntitySnowGolem EntitySnowGolemFlags = FlagsEntitySnowGolem.Pumpkin;
        public EntitySnowGolem()
        {

        }
        internal override void Serialize(Stream stream, Entity? rawDifference)
        {
            base.Serialize(stream, rawDifference);
            EntitySnowGolem? difference = rawDifference is EntitySnowGolem castDifference ? castDifference : null;
            if (difference is not null ? difference.EntitySnowGolemFlags != EntitySnowGolemFlags : true)
            {
                stream.WriteU8(16);
                stream.WriteU8(MetadataType.Byte);
                stream.WriteU8((byte)EntitySnowGolemFlags);
            }
        }
        public override void CloneFrom(Entity rawEntity)
        {
            base.CloneFrom(rawEntity);
            if (rawEntity is not EntitySnowGolem entity) return;
            EntitySnowGolemFlags = entity.EntitySnowGolemFlags;
        }
        public override EntitySnowGolem Clone()
        {
            EntitySnowGolem entity = new();
            entity.CloneFrom(this);
            return entity;
        }
    }
}
