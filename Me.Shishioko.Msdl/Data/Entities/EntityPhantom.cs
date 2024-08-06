using System.IO;
using Me.Shishioko.Msdl.Data.Protocol;
using Net.Myzuc.ShioLib;

namespace Me.Shishioko.Msdl.Data.Entities
{
    public sealed class EntityPhantom : EntityMob
    {
        internal override int Id => 76;
        public override double Height => 0.5 + 0.1 * Size;
        public override double Width => 0.9 + 0.2 * Size;
        public int Size = 0;
        public EntityPhantom()
        {

        }
        internal override void Serialize(Stream stream, EntityBase? rawDifference)
        {
            base.Serialize(stream, rawDifference);
            EntityPhantom? difference = rawDifference is EntityPhantom castDifference ? castDifference : null;
            if (difference is not null ? difference.Size != Size : true)
            {
                stream.WriteU8(16);
                stream.WriteS32V(MetadataType.S32V);
                stream.WriteS32V(Size);
            }
        }
        public override void Clone(EntityBase rawEntity)
        {
            base.Clone(rawEntity);
            if (rawEntity is not EntityPhantom entity) return;
            Size = entity.Size;
        }
    }
}
