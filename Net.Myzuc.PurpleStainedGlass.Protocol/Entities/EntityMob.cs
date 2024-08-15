using Net.Myzuc.PurpleStainedGlass.Protocol.Enums;
using Net.Myzuc.PurpleStainedGlass.Protocol.Const;
using Net.Myzuc.ShioLib;
using System.IO;

namespace Net.Myzuc.PurpleStainedGlass.Protocol.Entities
{
    public abstract class EntityMob : EntityLiving
    {
        public FlagsEntityMob EntityMobFlags = 0x00;
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
                stream.WriteU8((byte)EntityMobFlags);
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
