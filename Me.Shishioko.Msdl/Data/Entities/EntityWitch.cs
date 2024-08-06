using System.IO;
using Me.Shishioko.Msdl.Data.Protocol;
using Net.Myzuc.ShioLib;

namespace Me.Shishioko.Msdl.Data.Entities
{
    public sealed class EntityWitch : EntityMob
    {
        internal override int Id => 118;
        public override double Height => 1.95;
        public override double Width => 0.6;
        public bool Celebrating = false;
        public bool Drinking = false;
        public EntityWitch()
        {

        }
        internal override void Serialize(Stream stream, EntityBase? rawDifference)
        {
            base.Serialize(stream, rawDifference);
            EntityWitch? difference = rawDifference is EntityWitch castDifference ? castDifference : null;
            if (difference is not null ? difference.Celebrating != Celebrating : true)
            {
                stream.WriteU8(16);
                stream.WriteS32V(MetadataType.Bool);
                stream.WriteBool(Celebrating);
            }
            if (difference is not null ? difference.Drinking != Drinking : true)
            {
                stream.WriteU8(17);
                stream.WriteS32V(MetadataType.Bool);
                stream.WriteBool(Drinking);
            }
        }
        public override void Clone(EntityBase rawEntity)
        {
            base.Clone(rawEntity);
            if (rawEntity is not EntityWitch entity) return;
            Celebrating = entity.Celebrating;
            Drinking = entity.Drinking;
        }
    }
}
