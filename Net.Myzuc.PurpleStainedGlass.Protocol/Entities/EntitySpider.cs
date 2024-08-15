using System.IO;
using Net.Myzuc.PurpleStainedGlass.Protocol.Const;
using Net.Myzuc.ShioLib;

namespace Net.Myzuc.PurpleStainedGlass.Protocol.Entities
{
    public sealed class EntitySpider : EntityMob
    {
        internal override int Id => 100;
        public override double HitboxHeight => 0.9;
        public override double HitboxWidth => 1.4;
        public override bool HitboxSoftCollision => true;
        public override bool HitboxHardCollision => false;
        public override bool HitboxAlign => false;
        private byte EntitySpiderFlags = 0x01;
        public bool Climbing
        {
            get => (EntitySpiderFlags & 0x01) != 0;
            set
            {
                EntitySpiderFlags &= 0x01 ^ 0xFF;
                if (value) EntitySpiderFlags |= 0x01;
            }
        }
        public EntitySpider()
        {

        }
        internal override void Serialize(Stream stream, Entity? rawDifference)
        {
            base.Serialize(stream, rawDifference);
            EntitySpider? difference = rawDifference is EntitySpider castDifference ? castDifference : null;
            if (difference is not null ? difference.EntitySpiderFlags != EntitySpiderFlags : true)
            {
                stream.WriteU8(16);
                stream.WriteU8(MetadataType.Byte);
                stream.WriteU8(EntitySpiderFlags);
            }
        }
        public override void CloneFrom(Entity rawEntity)
        {
            base.CloneFrom(rawEntity);
            if (rawEntity is not EntitySpider entity) return;
            Climbing = entity.Climbing;
        }
        public override EntitySpider Clone()
        {
            EntitySpider entity = new();
            entity.CloneFrom(this);
            return entity;
        }
    }
}
