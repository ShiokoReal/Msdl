using Me.Shishioko.Msdl.Data.Protocol;
using Net.Myzuc.ShioLib;
using System.Drawing;
using System.IO;

namespace Me.Shishioko.Msdl.Data.Entities
{
    public abstract class EntityLiving : EntityBase
    {
        private byte EntityLivingFlags = 0x00;
        public bool HandActive
        {
            get => (EntityLivingFlags & 0x01) != 0;
            set
            {
                EntityLivingFlags &= 0x01 ^ 0xFF;
                if (value) EntityLivingFlags |= 0x01;
            }
        }
        public bool HandActiveOffhanded
        {
            get => (EntityLivingFlags & 0x02) != 0;
            set
            {
                EntityLivingFlags &= 0x02 ^ 0xFF;
                if (value) EntityLivingFlags |= 0x02;
            }
        }
        public bool Spinning
        {
            get => (EntityLivingFlags & 0x04) != 0;
            set
            {
                EntityLivingFlags &= 0x04 ^ 0xFF;
                if (value) EntityLivingFlags |= 0x04;
            }
        }
        public float Health = 1.0f;
        public (Color color, bool ambient)? PotionEffect = null; //TODO: fix and split and implement
        public int Arrows = 0;
        public int Stingers = 0;
        public Position? SleepingLocation = null;
        internal EntityLiving()
        {

        }
        internal override void Serialize(Stream stream, EntityBase? rawDifference)
        {
            base.Serialize(stream, rawDifference);
            EntityLiving? difference = rawDifference is EntityLiving castDifference ? castDifference : null;
            if (difference is not null ? difference.EntityLivingFlags != EntityLivingFlags : true)
            {
                stream.WriteU8(8);
                stream.WriteS32V(MetadataType.Byte);
                stream.WriteU8(EntityLivingFlags);
            }
            if (difference is not null ? difference.Health != Health : true)
            {
                stream.WriteU8(9);
                stream.WriteS32V(MetadataType.F32);
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
                stream.WriteS32V(MetadataType.S32V);
                stream.WriteS32V(Arrows);
            }
            if (difference is not null ? difference.Stingers != Stingers : true)
            {
                stream.WriteU8(13);
                stream.WriteS32V(MetadataType.S32V);
                stream.WriteS32V(Stingers);
            }
            if (difference is not null ? difference.SleepingLocation != SleepingLocation : true)
            {
                stream.WriteU8(14);
                stream.WriteS32V(MetadataType.LocatonN);
                stream.WriteBool(SleepingLocation.HasValue);
                if (SleepingLocation.HasValue) stream.WriteU64(SleepingLocation.Value.Data);
            }
        }
        public override void Clone(EntityBase rawEntity)
        {
            base.Clone(rawEntity);
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
