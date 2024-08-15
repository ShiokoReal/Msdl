using System.IO;
using Net.Myzuc.PurpleStainedGlass.Protocol.Items;
using Net.Myzuc.PurpleStainedGlass.Protocol.Const;
using Net.Myzuc.ShioLib;

namespace Net.Myzuc.PurpleStainedGlass.Protocol.Entities
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
