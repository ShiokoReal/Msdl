using Net.Myzuc.PurpleStainedGlass.Protocol.Enums;
using Net.Myzuc.PurpleStainedGlass.Protocol.Const;
using Net.Myzuc.ShioLib;
using System.IO;

namespace Net.Myzuc.PurpleStainedGlass.Protocol.Entities
{
    public sealed class EntityPlayer : EntityLiving
    {
        internal override int Id => 128;
        public override double HitboxHeight => 1.8;
        public override double HitboxWidth => 0.6;
        public override bool HitboxSoftCollision => true;
        public override bool HitboxHardCollision => false;
        public override bool HitboxAlign => false;
        public float Absorption = 0.0f;
        public int Score = 0;
        public FlagsSkin SkinFlags = 0x00;
        public bool Righthanded = true;
        //TODO: left and right shoulder parrot entity nbt
        public EntityPlayer()
        {

        }
        internal override void Serialize(Stream stream, Entity? rawDifference)
        {
            base.Serialize(stream, rawDifference);
            EntityPlayer? difference = rawDifference is EntityPlayer castDifference ? castDifference : null;
            if (difference is not null ? difference.Score != Score : true)
            {
                stream.WriteU8(15);
                stream.WriteU8(MetadataType.F32);
                stream.WriteF32(Absorption);
            }
            if (difference is not null ? difference.Score != Score : true)
            {
                stream.WriteU8(16);
                stream.WriteU8(MetadataType.S32V);
                stream.WriteS32V(Score);
            }
            if (difference is not null ? difference.SkinFlags != SkinFlags : true)
            {
                stream.WriteU8(17);
                stream.WriteU8(MetadataType.Byte);
                stream.WriteU8((byte)SkinFlags);
            }
            if (difference is not null ? difference.Righthanded != Righthanded : true)
            {
                stream.WriteU8(18);
                stream.WriteU8(MetadataType.Byte);
                stream.WriteBool(Righthanded);
            }
        }
        public override void CloneFrom(Entity rawEntity)
        {
            base.CloneFrom(rawEntity);
            if (rawEntity is not EntityPlayer entity) return;
            Absorption = entity.Absorption;
            Score = entity.Score;
            SkinFlags = entity.SkinFlags;
            Righthanded = entity.Righthanded;
        }
        public override EntityPlayer Clone()
        {
            EntityPlayer entity = new();
            entity.CloneFrom(this);
            return entity;
        }
    }
}
