using Net.Myzuc.ShioLib;
using System.Drawing;
using System.IO;

namespace Me.Shishioko.Msdl.Data.Entities
{
    public abstract class EntityLiving : EntityBase
    {
        public override EntityPose Pose
        {
            get => base.Pose;
            set
            {
                LivingEntityState &= 0x04 ^ 0xFF;
                if (value == EntityPose.Spinning) LivingEntityState |= 0x04;
                base.Pose = value;
            }
        }
        private byte LivingEntityState = 0;
        public bool? ActiveOffhanded
        {
            get 
            {
                if ((LivingEntityState & 0x01) == 0) return null;
                return (LivingEntityState & 0x02) != 0;
            }
            set
            {
                LivingEntityState &= 0x03 ^ 0xFF;
                if (value.HasValue) LivingEntityState |= 0x01;
                if (value.HasValue && value.Value) LivingEntityState |= 0x02;
            }
        }
        public float Health = 1.0f;
        public (Color color, bool ambient)? PotionEffect = null; //TODO: fix
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
            if (difference is not null ? difference.LivingEntityState != LivingEntityState : true)
            {
                stream.WriteU8(8);
                stream.WriteU8(0);
                stream.WriteU8(LivingEntityState);
            }
            if (difference is not null ? difference.Health != Health : true)
            {
                stream.WriteU8(9);
                stream.WriteU8(3);
                stream.WriteF32(Health);
            }
            /*if (difference is not null ? difference.PotionEffect != PotionEffect : true)
            {
                stream.WriteU8(10);
                stream.WriteU8(1);
                stream.WriteS32V(PotionEffect?.color.ToArgb() ?? 0);
                stream.WriteU8(11);
                stream.WriteU8(8);
                stream.WriteBool(PotionEffect?.ambient ?? false);
            }*/ //TODO: fix with up to date list type
            if (difference is not null ? difference.Arrows != Arrows : true)
            {
                stream.WriteU8(12);
                stream.WriteU8(1);
                stream.WriteS32V(Arrows);
            }
            if (difference is not null ? difference.Stingers != Stingers : true)
            {
                stream.WriteU8(13);
                stream.WriteU8(1);
                stream.WriteS32V(Stingers);
            }
            if (difference is not null ? difference.SleepingLocation != SleepingLocation : true)
            {
                stream.WriteU8(14);
                stream.WriteU8(11);
                stream.WriteBool(SleepingLocation.HasValue);
                if (SleepingLocation.HasValue) stream.WriteU64(SleepingLocation.Value.Data);
            }
        }
    }
}
