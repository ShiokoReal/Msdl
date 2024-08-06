using Me.Shishioko.Msdl.Data.Chat;
using Net.Myzuc.ShioLib;
using System.IO;

namespace Me.Shishioko.Msdl.Data.Entities
{
    public abstract class EntityBase
    {
        public enum EntityPose
        {
            Standing = 0,
            Elytra = 1,
            Sleeping = 2,
            Swimming = 3,
            Spinning = 4,
            Sneaking = 5,
            LongJumping = 6,
            Dead = 7,
            Croaking = 8,
            Tongue = 9,
            Sitting = 10,
            Roaring = 11,
            Sniffing = 12,
            Emerging = 13,
            Digging = 14,
        }
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
        public int Air = 300;
        public ChatComponent? DisplayName = null;
        public bool Silent = false;
        public bool Gravitationless = false;
        private EntityPose InternalPose = EntityPose.Standing;
        public virtual EntityPose Pose
        {
            get => InternalPose;
            set
            {
                EntityState &= 0x6D;
                if (value == EntityPose.Sneaking) EntityState |= 0x02;
                if (value == EntityPose.Swimming) EntityState |= 0x10;
                if (value == EntityPose.Elytra) EntityState |= 0x80;
                if (this is EntityLiving livingEntity)
                {

                }
                InternalPose = value;
            }
        }
        public int Freeze = 0;
        internal EntityBase()
        {

        }
        internal virtual void Serialize(Stream stream, EntityBase? difference)
        {
            if (difference is not null ? difference.EntityState != EntityState : true)
            {
                stream.WriteU8(0);
                stream.WriteU8(0);
                stream.WriteU8(EntityState);
            }
            if (difference is not null ? difference.Air != Air : true)
            {
                stream.WriteU8(1);
                stream.WriteU8(1);
                stream.WriteS32V(Air);
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
            if (difference is not null ? difference.Silent != Silent : true)
            {
                stream.WriteU8(4);
                stream.WriteU8(8);
                stream.WriteBool(Silent);
            }
            if (difference is not null ? difference.Gravitationless != Gravitationless : true)
            {
                stream.WriteU8(5);
                stream.WriteU8(8);
                stream.WriteBool(Gravitationless);
            }
            if (difference is not null ? difference.Pose != Pose : true)
            {
                stream.WriteU8(6);
                stream.WriteU8(21);
                stream.WriteS32V((int)Pose);
            }
            if (difference is not null ? difference.Freeze != Freeze : true)
            {
                stream.WriteU8(7);
                stream.WriteU8(1);
                stream.WriteS32V(Freeze);
            }
        }
        public virtual void Clone(EntityBase entity)
        {
            EntityState = entity.EntityState;
            Air = entity.Air;
            DisplayName = entity.DisplayName;
            Silent = entity.Silent;
            Gravitationless = entity.Gravitationless;
            InternalPose = entity.InternalPose;
            Freeze = entity.Freeze;
        }
    }
}
