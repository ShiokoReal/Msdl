using Me.Shishioko.Msdl.Data.Protocol;
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
        internal override void Serialize(Stream stream, Entity? rawDifference)
        {
            base.Serialize(stream, rawDifference);
            EntityGrowable? difference = rawDifference is EntityGrowable castDifference ? castDifference : null;
            if (difference is not null ? difference.Baby != Baby : true)
            {
                stream.WriteU8(16);
                stream.WriteU8(MetadataType.Bool);
                stream.WriteBool(Baby);
            }
        }
        public override void Clone(Entity rawEntity)
        {
            base.Clone(rawEntity);
            if (rawEntity is not EntityGrowable entity) return;
            Baby = entity.Baby;
        }
    }
}
