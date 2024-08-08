using System.IO;
using Me.Shishioko.Msdl.Data.Protocol;
using Net.Myzuc.ShioLib;

namespace Me.Shishioko.Msdl.Data.Entities
{
    public sealed class EntitySnowGolem : EntityMob
    {
        internal override int Id => 96;
        public override double HitboxHeight => 1.9;
        public override double HitboxWidth => 0.7;
        public override bool HitboxSoftCollision => true;
        public override bool HitboxHardCollision => false;
        public override bool HitboxAlign => false;
        private byte EntitySnowGolemFlags = 0x10;
        public bool Pumpkin
        {
            get => (EntitySnowGolemFlags & 0x10) != 0;
            set
            {
                EntitySnowGolemFlags &= 0x10 ^ 0xFF;
                if (value) EntitySnowGolemFlags |= 0x10;
            }
        }
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
                stream.WriteU8(EntitySnowGolemFlags);
            }
        }
        public override void Clone(Entity rawEntity)
        {
            base.Clone(rawEntity);
            if (rawEntity is not EntitySnowGolem entity) return;
            EntitySnowGolemFlags = entity.EntitySnowGolemFlags;
        }
    }
}
