using Me.Shishioko.Msdl.Data.Protocol;
using Net.Myzuc.ShioLib;
using System.IO;

namespace Me.Shishioko.Msdl.Data.Entities
{
    public sealed class EntityPlayer : EntityLiving
    {
        public struct EntityPlayerSkinMask
        {
            public byte Mask;
            public bool Cape
            {
                get => (Mask & 0x01) != 0;
                set => Mask = (byte)((Mask & ~0x01) | (value ? 0x01 : 0x00));
            }
            public bool Body
            {
                get => (Mask & 0x02) != 0;
                set => Mask = (byte)((Mask & ~0x02) | (value ? 0x02 : 0x00));
            }
            public bool ArmLeft
            {
                get => (Mask & 0x04) != 0;
                set => Mask = (byte)((Mask & ~0x04) | (value ? 0x04 : 0x00));
            }
            public bool ArmRight
            {
                get => (Mask & 0x08) != 0;
                set => Mask = (byte)((Mask & ~0x08) | (value ? 0x08 : 0x00));
            }
            public bool LegLeft
            {
                get => (Mask & 0x10) != 0;
                set => Mask = (byte)((Mask & ~0x10) | (value ? 0x10 : 0x00));
            }
            public bool LegRight
            {
                get => (Mask & 0x20) != 0;
                set => Mask = (byte)((Mask & ~0x20) | (value ? 0x20 : 0x00));
            }
            public bool Hat
            {
                get => (Mask & 0x40) != 0;
                set => Mask = (byte)((Mask & ~0x40) | (value ? 0x40 : 0x00));
            }
            public EntityPlayerSkinMask()
            {
                Mask = 0x7F;
            }
            public EntityPlayerSkinMask(byte mask)
            {
                Mask = mask;
            }
        }
        internal override int Id => 128;
        public override double HitboxHeight => 1.8;
        public override double HitboxWidth => 0.6;
        public override bool HitboxSoftCollision => true;
        public override bool HitboxHardCollision => false;
        public override bool HitboxAlign => false;
        public float Absorption = 0.0f;
        public int Score = 0;
        public EntityPlayerSkinMask SkinMask = new(0);
        public bool Righthanded = true;
        //TODO: left and right shoulder parrot entity nbt
        public EntityPlayer()
        {

        }
        internal override void Serialize(Stream stream, EntityBase? rawDifference)
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
            if (difference is not null ? difference.SkinMask.Mask != SkinMask.Mask : true)
            {
                stream.WriteU8(17);
                stream.WriteU8(MetadataType.Byte);
                stream.WriteU8(SkinMask.Mask);
            }
            if (difference is not null ? difference.Righthanded != Righthanded : true)
            {
                stream.WriteU8(18);
                stream.WriteU8(MetadataType.Byte);
                stream.WriteBool(Righthanded);
            }
        }
        public override void Clone(EntityBase rawEntity)
        {
            base.Clone(rawEntity);
            if (rawEntity is not EntityPlayer entity) return;
            Absorption = entity.Absorption;
            Score = entity.Score;
            SkinMask = entity.SkinMask;
            Righthanded = entity.Righthanded;
        }
    }
}
