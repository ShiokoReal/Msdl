using Me.Shishioko.Msdl.Data.Chat;
using Me.Shishioko.Msdl.Data.Entity.Properties;
using Net.Myzuc.ShioLib;
using System.IO;

namespace Me.Shishioko.Msdl.Data.Entity.Classes
{
    public abstract class EntityClassBase
    {
        public abstract int Id { get; }
        public abstract double Height { get; }
        public abstract double Width { get; }
        public virtual int InitialData => 0;
        private byte EntityState = 0;
        public bool Burning
        {
            get => (EntityState & 0x01) != 0;
            set
            {
                EntityState &= 0x01 ^ 0xFF;
                if (value) EntityState |= 0x01;
            }
        }
        public bool Invisible
        {
            get => (EntityState & 0x20) != 0;
            set
            {
                EntityState &= 0x20 ^ 0xFF;
                if (value) EntityState |= 0x20;
            }
        }
        public bool Glowing
        {
            get => (EntityState & 0x40) != 0;
            set
            {
                EntityState &= 0x40 ^ 0xFF;
                if (value) EntityState |= 0x40;
            }
        }
        public bool ElytraFlying //TODO: used by client? or the other elytra flyin in pose?
        {
            get => (EntityState & 0x80) != 0;
            set
            {
                EntityState &= 0x80 ^ 0xFF;
                if (value) EntityState |= 0x80;
            }
        }
        public ChatComponent? DisplayName = null;
        public EntityPose Pose = EntityPose.Standing;
        internal EntityClassBase()
        {

        }
        internal virtual void Serialize(Stream stream, EntityClassBase? difference)
        {
            if (difference is not null ? difference.EntityState != EntityState : true)
            {
                stream.WriteU8(0);
                stream.WriteU8(0);
                stream.WriteU8(EntityState);
            }
            if (difference is not null ? difference.DisplayName != DisplayName : true)
            {
                stream.WriteU8(2);
                stream.WriteU8(6);
                stream.WriteBool(DisplayName is not null);
                if (DisplayName is not null)
                {
                    stream.WriteU8(0x0A);
                    DisplayName.Serialize(stream);
                }
                stream.WriteU8(3);
                stream.WriteU8(8);
                stream.WriteBool(DisplayName is not null);
            }
            if (difference is not null ? difference.Pose != Pose : true)
            {
                stream.WriteU8(6);
                stream.WriteU8(21);
                stream.WriteS32V((int)Pose);
            }
        }
    }
}
