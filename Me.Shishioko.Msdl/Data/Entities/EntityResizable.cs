using System.IO;
using Me.Shishioko.Msdl.Data.Protocol;
using Net.Myzuc.ShioLib;

namespace Me.Shishioko.Msdl.Data.Entities
{
    public abstract class EntityResizable : EntityMob
    {
        public int Size = 0;
        internal EntityResizable()
        {

        }
        internal override void Serialize(Stream stream, Entity? rawDifference)
        {
            base.Serialize(stream, rawDifference);
            EntityResizable? difference = rawDifference is EntityResizable castDifference ? castDifference : null;
            if (difference is not null ? difference.Size != Size : true)
            {
                stream.WriteU8(16);
                stream.WriteU8(MetadataType.S32V);
                stream.WriteS32V(Size);
            }
        }
        public override void Clone(Entity rawEntity)
        {
            base.Clone(rawEntity);
            if (rawEntity is not EntityResizable entity) return;
            Size = entity.Size;
        }
    }
}
