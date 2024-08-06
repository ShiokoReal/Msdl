using System.IO;
using Me.Shishioko.Msdl.Data.Protocol;
using Net.Myzuc.ShioLib;

namespace Me.Shishioko.Msdl.Data.Entities
{
    public sealed class EntitySnowGolem : EntityMob
    {
        internal override int Id => 96;
        public override double Height => 1.9;
        public override double Width => 0.7;
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
        internal override void Serialize(Stream stream, EntityBase? rawDifference)
        {
            base.Serialize(stream, rawDifference);
            EntitySnowGolem? difference = rawDifference is EntitySnowGolem castDifference ? castDifference : null;
            if (difference is not null ? difference.EntitySnowGolemFlags != EntitySnowGolemFlags : true)
            {
                stream.WriteU8(16);
                stream.WriteS32V(MetadataType.Byte);
                stream.WriteU8(EntitySnowGolemFlags);
            }
        }
        public override void Clone(EntityBase rawEntity)
        {
            base.Clone(rawEntity);
            if (rawEntity is not EntitySnowGolem entity) return;
            EntitySnowGolemFlags = entity.EntitySnowGolemFlags;
        }
    }
}
