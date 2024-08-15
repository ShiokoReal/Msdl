using Net.Myzuc.PurpleStainedGlass.Protocol.Enums;
using Net.Myzuc.PurpleStainedGlass.Protocol.Objects.Chat;
using Net.Myzuc.PurpleStainedGlass.Protocol.Const;
using Net.Myzuc.ShioLib;
using System.IO;

namespace Net.Myzuc.PurpleStainedGlass.Protocol.Entities
{
    public abstract class Entity
    {
        internal abstract int Id { get; }
        public abstract double HitboxHeight { get; }
        public abstract double HitboxWidth { get; }
        public abstract bool HitboxSoftCollision { get; }
        public abstract bool HitboxHardCollision { get; }
        public abstract bool HitboxAlign { get; }
        public FlagsEntity EntityFlags = 0x00;
        public int Air = 300;
        public ChatComponent? Name = null;
        public bool NameVisibility = false;
        public bool Silent = false;
        public bool Gravitationless = false;
        public EnumPose Pose = EnumPose.Standing;
        public int Freeze = 0;
        internal Entity()
        {

        }
        internal virtual void Serialize(Stream stream, Entity? difference)
        {
            if (difference is not null ? difference.EntityFlags != EntityFlags : true)
            {
                stream.WriteU8(0);
                stream.WriteU8(MetadataType.Byte);
                stream.WriteU8((byte)EntityFlags);
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
                if (Name is not null) Name.NbtSerialize(stream);
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
            EntityFlags = entity.EntityFlags;
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
