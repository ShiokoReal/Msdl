using Net.Myzuc.PurpleStainedGlass.Protocol.Enums;
using Net.Myzuc.PurpleStainedGlass.Protocol.Const;
using Net.Myzuc.PurpleStainedGlass.Protocol.Structs;
using Net.Myzuc.ShioLib;
using System.Drawing;
using System.IO;

namespace Net.Myzuc.PurpleStainedGlass.Protocol.Entities
{
    public abstract class EntityLiving : Entity
    {
        public FlagsEntityLiving EntityLivingFlags = 0x00;
        public float Health = 1.0f;
        public (Color color, bool ambient)? PotionEffect = null; //TODO: fix and split and implement
        public int Arrows = 0;
        public int Stingers = 0;
        public Location? SleepingLocation = null;
        internal EntityLiving()
        {

        }
        internal override void Serialize(Stream stream, Entity? rawDifference)
        {
            base.Serialize(stream, rawDifference);
            EntityLiving? difference = rawDifference is EntityLiving castDifference ? castDifference : null;
            if (difference is not null ? difference.EntityLivingFlags != EntityLivingFlags : true)
            {
                stream.WriteU8(8);
                stream.WriteU8(MetadataType.Byte);
                stream.WriteU8((byte)EntityLivingFlags);
            }
            if (difference is not null ? difference.Health != Health : true)
            {
                stream.WriteU8(9);
                stream.WriteU8(MetadataType.F32);
                stream.WriteF32(Health);
            }
            /*if (difference is not null ? difference.PotionEffect != PotionEffect : true)
            {
                stream.WriteU8(10);
                //stream.WriteU8(1);
                stream.WriteS32V(PotionEffect?.color.ToArgb() ?? 0);
                stream.WriteU8(11);
                //stream.WriteU8(8);
                stream.WriteBool(PotionEffect?.ambient ?? false);
            }*/ //TODO: fix with up to date list type
            if (difference is not null ? difference.Arrows != Arrows : true)
            {
                stream.WriteU8(12);
                stream.WriteU8(MetadataType.S32V);
                stream.WriteS32V(Arrows);
            }
            if (difference is not null ? difference.Stingers != Stingers : true)
            {
                stream.WriteU8(13);
                stream.WriteU8(MetadataType.S32V);
                stream.WriteS32V(Stingers);
            }
            if (difference is not null ? difference.SleepingLocation != SleepingLocation : true)
            {
                stream.WriteU8(14);
                stream.WriteU8(MetadataType.LocatonN);
                stream.WriteBool(SleepingLocation.HasValue);
                if (SleepingLocation.HasValue) stream.WriteU64(SleepingLocation.Value.Data);
            }
        }
        public override void CloneFrom(Entity rawEntity)
        {
            base.CloneFrom(rawEntity);
            if (rawEntity is not EntityLiving entity) return;
            EntityLivingFlags = entity.EntityLivingFlags;
            Health = entity.Health;
            PotionEffect = entity.PotionEffect;
            Arrows = entity.Arrows;
            Stingers = entity.Stingers;
            SleepingLocation = entity.SleepingLocation;
        }
    }
}
