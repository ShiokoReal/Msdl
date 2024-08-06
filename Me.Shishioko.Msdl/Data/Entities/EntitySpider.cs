using System.IO;
using Me.Shishioko.Msdl.Data.Protocol;
using Net.Myzuc.ShioLib;

namespace Me.Shishioko.Msdl.Data.Entities
{
    public sealed class EntitySpider : EntityMob
    {
        internal override int Id => 100;
        public override double Height => 0.9;
        public override double Width => 1.4;
        public bool Climbing = false;
        public EntitySpider()
        {

        }
        internal override void Serialize(Stream stream, EntityBase? rawDifference)
        {
            base.Serialize(stream, rawDifference);
            EntitySpider? difference = rawDifference is EntitySpider castDifference ? castDifference : null;
            if (difference is not null ? difference.Climbing != Climbing : true)
            {
                stream.WriteU8(16);
                stream.WriteU8(MetadataType.Bool);
                stream.WriteBool(Climbing);
            }
        }
        public override void Clone(EntityBase rawEntity)
        {
            base.Clone(rawEntity);
            if (rawEntity is not EntitySpider entity) return;
            Climbing = entity.Climbing;
        }
    }
}
