using System.IO;
using Me.Shishioko.Msdl.Data.Protocol;
using Net.Myzuc.ShioLib;

namespace Me.Shishioko.Msdl.Data.Entities
{
    public sealed class EntityWitch : EntityMob
    {
        internal override int Id => 118;
        public override double HitboxHeight => 1.95;
        public override double HitboxWidth => 0.6;
        public override bool HitboxSoftCollision => true;
        public override bool HitboxHardCollision => false;
        public override bool HitboxAlign => false;
        public bool Celebrating = false;
        public bool Drinking = false;
        public EntityWitch()
        {

        }
        internal override void Serialize(Stream stream, Entity? rawDifference)
        {
            base.Serialize(stream, rawDifference);
            EntityWitch? difference = rawDifference is EntityWitch castDifference ? castDifference : null;
            if (difference is not null ? difference.Celebrating != Celebrating : true)
            {
                stream.WriteU8(16);
                stream.WriteU8(MetadataType.Bool);
                stream.WriteBool(Celebrating);
            }
            if (difference is not null ? difference.Drinking != Drinking : true)
            {
                stream.WriteU8(17);
                stream.WriteU8(MetadataType.Bool);
                stream.WriteBool(Drinking);
            }
        }
        public override void Clone(Entity rawEntity)
        {
            base.Clone(rawEntity);
            if (rawEntity is not EntityWitch entity) return;
            Celebrating = entity.Celebrating;
            Drinking = entity.Drinking;
        }
    }
}
