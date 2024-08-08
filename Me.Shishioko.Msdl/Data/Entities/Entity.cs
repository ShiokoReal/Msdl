using Me.Shishioko.Msdl.Data.Chat;
using Me.Shishioko.Msdl.Data.Protocol;
using Net.Myzuc.ShioLib;
using System.IO;

namespace Me.Shishioko.Msdl.Data.Entities
{
    public abstract class Entity
    {
        public enum EntityBasePose
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
        internal abstract int Id { get; }
        public abstract double HitboxHeight { get; }
        public abstract double HitboxWidth { get; }
        public abstract bool HitboxSoftCollision { get; }
        public abstract bool HitboxHardCollision { get; }
        public abstract bool HitboxAlign { get; }

        private byte EntityBaseFlags = 0x00;
        public bool Burning
        {
            get => (EntityBaseFlags & 0x01) != 0;
            set
            {
                EntityBaseFlags &= 0x01 ^ 0xFF;
                if (value) EntityBaseFlags |= 0x01;
            }
        }
        public bool Sneaking
        {
            get => (EntityBaseFlags & 0x02) != 0;
            set
            {
                EntityBaseFlags &= 0x02 ^ 0xFF;
                if (value) EntityBaseFlags |= 0x02;
            }
        }
        public bool Sprinting
        {
            get => (EntityBaseFlags & 0x08) != 0;
            set
            {
                EntityBaseFlags &= 0x08 ^ 0xFF;
                if (value) EntityBaseFlags |= 0x08;
            }
        }
        public bool Swimming
        {
            get => (EntityBaseFlags & 0x10) != 0;
            set
            {
                EntityBaseFlags &= 0x10 ^ 0xFF;
                if (value) EntityBaseFlags |= 0x10;
            }
        }
        public bool Invisible
        {
            get => (EntityBaseFlags & 0x20) != 0;
            set
            {
                EntityBaseFlags &= 0x20 ^ 0xFF;
                if (value) EntityBaseFlags |= 0x20;
            }
        }
        public bool Glowing
        {
            get => (EntityBaseFlags & 0x40) != 0;
            set
            {
                EntityBaseFlags &= 0x40 ^ 0xFF;
                if (value) EntityBaseFlags |= 0x40;
            }
        }
        public bool ElytraFlying
        {
            get => (EntityBaseFlags & 0x80) != 0;
            set
            {
                EntityBaseFlags &= 0x80 ^ 0xFF;
                if (value) EntityBaseFlags |= 0x80;
            }
        }
        public int Air = 300;
        public ChatComponent? Name = null;
        public bool NameVisibility = false;
        public bool Silent = false;
        public bool Gravitationless = false;
        public EntityBasePose Pose = EntityBasePose.Standing;
        public int Freeze = 0;
        internal Entity()
        {

        }
        internal virtual void Serialize(Stream stream, Entity? difference)
        {
            if (difference is not null ? difference.EntityBaseFlags != EntityBaseFlags : true)
            {
                stream.WriteU8(0);
                stream.WriteU8(MetadataType.Byte);
                stream.WriteU8(EntityBaseFlags);
            }
            if (difference is not null ? difference.Air != Air : true)
            {
                stream.WriteU8(1);
                stream.WriteU8(MetadataType.S32V);
                stream.WriteS32V(Air);
            }
            if (difference is not null ? difference.Name != Name : true)
            {
                stream.WriteU8(2);
                stream.WriteU8(MetadataType.ChatComponentN);
                stream.WriteBool(Name is not null);
                if (Name is not null)
                {
                    stream.WriteU8(0x0A);
                    Name.Serialize(stream);
                }
            }
            if (difference is not null ? difference.NameVisibility != NameVisibility : true)
            {
                stream.WriteU8(3);
                stream.WriteU8(MetadataType.Bool);
                stream.WriteBool(NameVisibility);
            }
            if (difference is not null ? difference.Silent != Silent : true)
            {
                stream.WriteU8(4);
                stream.WriteU8(MetadataType.Bool);
                stream.WriteBool(Silent);
            }
            if (difference is not null ? difference.Gravitationless != Gravitationless : true)
            {
                stream.WriteU8(5);
                stream.WriteU8(MetadataType.Bool);
                stream.WriteBool(Gravitationless);
            }
            if (difference is not null ? difference.Pose != Pose : true)
            {
                stream.WriteU8(6);
                stream.WriteU8(MetadataType.EntityBasePose);
                stream.WriteS32V((int)Pose);
            }
            if (difference is not null ? difference.Freeze != Freeze : true)
            {
                stream.WriteU8(7);
                stream.WriteU8(MetadataType.S32V);
                stream.WriteS32V(Freeze);
            }
        }
        public virtual void CloneFrom(Entity entity)
        {
            EntityBaseFlags = entity.EntityBaseFlags;
            Air = entity.Air;
            Name = entity.Name;
            NameVisibility = entity.NameVisibility;
            Silent = entity.Silent;
            Gravitationless = entity.Gravitationless;
            Pose = entity.Pose;
            Freeze = entity.Freeze;
        }
        public abstract Entity Clone();
    }
}
