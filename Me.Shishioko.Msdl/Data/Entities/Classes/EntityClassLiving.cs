using Net.Myzuc.ShioLib;
using System.Drawing;
using System.IO;
using System;

namespace Me.Shishioko.Msdl.Data.Entity.Classes
{
    public abstract class EntityClassLiving : EntityClassBase
    {
        private byte LivingEntityState = 0;
        public bool? ActiveOffhanded
        {
            get 
            {
                if ((LivingEntityState & 0x01) == 0) return false;
                return (LivingEntityState & 0x02) != 0;
            }
            set
            {
                LivingEntityState &= 0x03 ^ 0xFF;
                if (value.HasValue) LivingEntityState |= 0x01;
                if (value.HasValue && value.Value) LivingEntityState |= 0x02;
            }
        }
        public bool Spinning
        {
            get => (LivingEntityState & 0x04) != 0;
            set
            {
                LivingEntityState &= 0x04 ^ 0xFF;
                if (value) LivingEntityState |= 0x04;
            }
        }
        public (Color color, bool ambient)? PotionEffect = null;
        public int Arrows = 0;
        public int Stingers = 0;
        public Position? SleepingLocation = null;
        internal EntityClassLiving()
        {

        }
        internal override void Serialize(Stream stream, EntityClassBase? rawDifference)
        {
            base.Serialize(stream, rawDifference);
            EntityClassLiving? difference = null;
            if (rawDifference is EntityClassLiving castDifference) difference = castDifference;
            else if (rawDifference is not null) throw new ArgumentException();
            if (difference is not null ? difference.LivingEntityState != LivingEntityState : true)
            {
                stream.WriteU8(8);
                stream.WriteU8(0);
                stream.WriteU8(LivingEntityState);
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
