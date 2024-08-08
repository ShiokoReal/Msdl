using Me.Shishioko.Msdl.Data.Protocol;
using Net.Myzuc.ShioLib;
using System.IO;

namespace Me.Shishioko.Msdl.Data.Entities
{
    public abstract class EntityMob : EntityLiving
    {
        private byte EntityMobFlags = 0x00;
        public bool NoAI
        {
            get => (EntityMobFlags & 0x01) != 0;
            set
            {
                EntityMobFlags &= 0x01 ^ 0xFF;
                if (value) EntityMobFlags |= 0x01;
            }
        }
        public bool Lefthanded
        {
            get => (EntityMobFlags & 0x02) != 0;
            set
            {
                EntityMobFlags &= 0x02 ^ 0xFF;
                if (value) EntityMobFlags |= 0x02;
            }
        }
        public bool Aggressive
        {
            get => (EntityMobFlags & 0x04) != 0;
            set
            {
                EntityMobFlags &= 0x04 ^ 0xFF;
                if (value) EntityMobFlags |= 0x04;
            }
        }
        public EntityMob()
        {

        }
        internal override void Serialize(Stream stream, Entity? rawDifference)
        {
            base.Serialize(stream, rawDifference);
            EntityMob? difference = rawDifference is EntityMob castDifference ? castDifference : null;
            if (difference is not null ? difference.EntityMobFlags != EntityMobFlags : true)
            {
                stream.WriteU8(15);
                stream.WriteU8(MetadataType.Byte);
                stream.WriteU8(EntityMobFlags);
            }
        }
        public override void CloneFrom(Entity rawEntity)
        {
            base.CloneFrom(rawEntity);
            if (rawEntity is not EntityMob entity) return;
            EntityMobFlags = entity.EntityMobFlags;
        }
    }
}
