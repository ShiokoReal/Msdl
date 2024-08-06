using System.IO;
using Me.Shishioko.Msdl.Data.Protocol;
using Net.Myzuc.ShioLib;

namespace Me.Shishioko.Msdl.Data.Entities
{
    public sealed class EntityGhast : EntityMob
    {
        internal override int Id => 45;
        public override double Height => 4;
        public override double Width => 4;
        public bool Charging = false;
        public EntityGhast()
        {

        }
        internal override void Serialize(Stream stream, EntityBase? rawDifference)
        {
            base.Serialize(stream, rawDifference);
            EntityGhast? difference = rawDifference is EntityGhast castDifference ? castDifference : null;
            if (difference is not null ? difference.Charging != Charging : true)
            {
                stream.WriteU8(16);
                stream.WriteU8(MetadataType.Bool);
                stream.WriteBool(Charging);
            }
        }
        public override void Clone(EntityBase rawEntity)
        {
            base.Clone(rawEntity);
            if (rawEntity is not EntityGhast entity) return;
            Charging = entity.Charging;
        }
    }
}
