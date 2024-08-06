using Net.Myzuc.ShioLib;
using System;
using System.IO;
using static System.Formats.Asn1.AsnWriter;

namespace Me.Shishioko.Msdl.Data.Entities
{
    public sealed class EntityPlayer : EntityLiving
    {
        public override int Id => 128;
        public override double Height => 1.8;
        public override double Width => 0.6;
        public float Absorption = 0.0f;
        public int Score = 0;
        public SkinMask SkinMask = new(0);
        public bool Righthanded = true;
        //TODO: left and right shoulder parrot entity nbt
        public EntityPlayer()
        {

        }
        internal override void Serialize(Stream stream, EntityBase? rawDifference)
        {
            base.Serialize(stream, rawDifference);
            EntityPlayer? difference = null;
            if (rawDifference is EntityPlayer castDifference) difference = castDifference;
            else if (rawDifference is not null) throw new ArgumentException();
            if (difference is not null ? difference.Score != Score : true)
            {
                stream.WriteU8(15);
                stream.WriteU8(3);
                stream.WriteF32(Absorption);
            }
            if (difference is not null ? difference.Score != Score : true)
            {
                stream.WriteU8(16);
                stream.WriteU8(1);
                stream.WriteS32V(Score);
            }
            if (difference is not null ? difference.SkinMask.Mask != SkinMask.Mask : true)
            {
                stream.WriteU8(17);
                stream.WriteU8(0);
                stream.WriteU8(SkinMask.Mask);
            }
            if (difference is not null ? difference.Righthanded != Righthanded : true)
            {
                stream.WriteU8(18);
                stream.WriteU8(0);
                stream.WriteBool(Righthanded);
            }
        }
    }
}
