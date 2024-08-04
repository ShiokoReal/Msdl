using Me.Shishioko.Msdl.Data.Entity.Classes;
using Net.Myzuc.ShioLib;
using System;
using System.IO;

namespace Me.Shishioko.Msdl.Data.Entities
{
    public sealed class EntityPlayer : EntityClassLiving
    {
        public override int Id => 128;
        public override double Height => 1.8;
        public override double Width => 0.6;
        public SkinMask SkinMask;
        public bool Righthanded;
        //TODO: left and right shoulder parrot entity nbt
        public EntityPlayer()
        {

        }
        internal override void Serialize(Stream stream, EntityClassBase? rawDifference)
        {
            base.Serialize(stream, rawDifference);
            EntityPlayer? difference = null;
            if (rawDifference is EntityPlayer castDifference) difference = castDifference;
            else if (rawDifference is not null) throw new ArgumentException();
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
