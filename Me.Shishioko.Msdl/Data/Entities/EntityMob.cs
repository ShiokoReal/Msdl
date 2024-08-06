using Net.Myzuc.ShioLib;
using System.IO;

namespace Me.Shishioko.Msdl.Data.Entities
{
    public abstract class EntityMob : EntityLiving
    {
        private byte MobFlags = 0;
        public bool NoAI
        {
            get => (MobFlags & 0x01) != 0;
            set
            {
                MobFlags &= 0x01 ^ 0xFF;
                if (value) MobFlags |= 0x01;
            }
        }
        public bool Lefthanded
        {
            get => (MobFlags & 0x02) != 0;
            set
            {
                MobFlags &= 0x02 ^ 0xFF;
                if (value) MobFlags |= 0x02;
            }
        }
        public bool Aggressive
        {
            get => (MobFlags & 0x04) != 0;
            set
            {
                MobFlags &= 0x04 ^ 0xFF;
                if (value) MobFlags |= 0x04;
            }
        }
        public EntityMob()
        {

        }
        internal override void Serialize(Stream stream, EntityBase? rawDifference)
        {
            base.Serialize(stream, rawDifference);
            EntityMob? difference = rawDifference is EntityMob castDifference ? castDifference : null;
            if (difference is not null ? difference.MobFlags != MobFlags : true)
            {
                stream.WriteU8(15);
                stream.WriteU8(0);
                stream.WriteU8(MobFlags);
            }
        }
    }
}
