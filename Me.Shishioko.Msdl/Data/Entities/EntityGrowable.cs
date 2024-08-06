using Net.Myzuc.ShioLib;
using System.IO;

namespace Me.Shishioko.Msdl.Data.Entities
{
    public abstract class EntityGrowable : EntityMob
    {
        public bool Baby = false;
        public EntityGrowable()
        {

        }
        internal override void Serialize(Stream stream, EntityBase? rawDifference)
        {
            base.Serialize(stream, rawDifference);
            EntityGrowable? difference = rawDifference is EntityGrowable castDifference ? castDifference : null;
            if (difference is not null ? difference.Baby != Baby : true)
            {
                stream.WriteU8(16);
                stream.WriteU8(8);
                stream.WriteBool(Baby);
            }
        }
    }
}
