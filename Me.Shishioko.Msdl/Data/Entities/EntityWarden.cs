using System.IO;
using Me.Shishioko.Msdl.Data.Protocol;
using Net.Myzuc.ShioLib;

namespace Me.Shishioko.Msdl.Data.Entities
{
    public sealed class EntityWarden : EntityMob
    {
        internal override int Id => 116;
        public override double Height => 2.9;
        public override double Width => 0.9;
        public int Anger = 0;
        public EntityWarden()
        {

        }
        internal override void Serialize(Stream stream, EntityBase? rawDifference)
        {
            base.Serialize(stream, rawDifference);
            EntityWarden? difference = rawDifference is EntityWarden castDifference ? castDifference : null;
            if (difference is not null ? difference.Anger != Anger : true)
            {
                stream.WriteU8(16);
                stream.WriteU8(MetadataType.S32V);
                stream.WriteS32V(Anger);
            }
        }
        public override void Clone(EntityBase rawEntity)
        {
            base.Clone(rawEntity);
            if (rawEntity is not EntityWarden entity) return;
            Anger = entity.Anger;
        }
    }
}
