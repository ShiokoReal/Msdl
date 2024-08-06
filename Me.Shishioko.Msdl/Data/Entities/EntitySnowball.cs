using System.IO;
using Net.Myzuc.ShioLib;

namespace Me.Shishioko.Msdl.Data.Entities
{
    public sealed class EntitySnowball : EntityBase
    {
        internal override int Id => 97;
        public override double HitboxHeight => 0.25;
        public override double HitboxWidth => 0.25;
        public override bool HitboxSoftCollision => false;
        public override bool HitboxHardCollision => false;
        public override bool HitboxAlign => false;
        public Item Item = new()
        {
            ID = 912,
            Count = 1,
        }; //TODO: read snowball id from enum item list
        public EntitySnowball()
        {

        }
        internal override void Serialize(Stream stream, EntityBase? rawDifference)
        {
            base.Serialize(stream, rawDifference);
            EntitySnowball? difference = rawDifference is EntitySnowball castDifference ? castDifference : null;
            if (difference is not null ? difference.Item != Item : true)
            {
                stream.WriteU8(8);
                stream.WriteU8(7);
                Item.Serialize(stream);
            }
        }
        public override void Clone(EntityBase rawEntity)
        {
            base.Clone(rawEntity);
            if (rawEntity is not EntitySnowball entity) return;
            Item.Clone(entity.Item);
        }
    }
}
