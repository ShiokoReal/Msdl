using Me.Shishioko.Msdl.Data.Entity.Classes;
using Net.Myzuc.ShioLib;
using System;
using System.IO;

namespace Me.Shishioko.Msdl.Data.Entities.Classes
{
    public abstract class EntityClassMob : EntityClassLiving
    {
        public bool Lefthanded = false;
        public EntityClassMob()
        {

        }
        internal override void Serialize(Stream stream, EntityClassBase? rawDifference)
        {
            base.Serialize(stream, rawDifference);
            EntityClassMob? difference = null;
            if (rawDifference is EntityClassMob castDifference) difference = castDifference;
            else if (rawDifference is not null) throw new ArgumentException();
            if (difference is not null ? difference.Lefthanded != Lefthanded : true)
            {
                stream.WriteU8(15);
                stream.WriteU8(0);
                stream.WriteU8((byte)(Lefthanded ? 0x02 : 0x00));
            }
        }
    }
}
