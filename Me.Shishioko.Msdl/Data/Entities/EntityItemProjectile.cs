using System.IO;
using Me.Shishioko.Msdl.Data.Items;
using Me.Shishioko.Msdl.Data.Protocol;
using Net.Myzuc.ShioLib;

namespace Me.Shishioko.Msdl.Data.Entities
{
    public abstract class EntityItemProjectile : Entity
    {
        public Item Item;
        internal EntityItemProjectile(Item item)
        {
            Item = item;
        }
        internal override void Serialize(Stream stream, Entity? rawDifference)
        {
            base.Serialize(stream, rawDifference);
            EntityItemProjectile? difference = rawDifference is EntityItemProjectile castDifference ? castDifference : null;
            if (difference is not null ? difference.Item != Item : true)
            {
                stream.WriteU8(8);
                stream.WriteU8(MetadataType.Item);
                Item.Serialize(stream);
            }
        }
        public override void CloneFrom(Entity rawEntity)
        {
            base.CloneFrom(rawEntity);
            if (rawEntity is not EntityItemProjectile entity) return;
            Item = entity.Item.Clone();
        }
    }
}
