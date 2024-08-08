using System;
using System.IO;
using Me.Shishioko.Msdl.Data.Protocol;
using Net.Myzuc.ShioLib;

namespace Me.Shishioko.Msdl.Data.Entities
{
    public sealed class EntityShulker : EntityMob
    {
        internal override int Id => 88;
        public override double HitboxHeight => 1.0;
        public override double HitboxWidth => 1.0;
        public override bool HitboxSoftCollision => true;
        public override bool HitboxHardCollision => true;
        public override bool HitboxAlign => true;
        public BlockFace Face = BlockFace.Bottom;
        public byte Peek = 0;
        public ConsoleColor? Color = null;
        public EntityShulker()
        {

        }
        internal override void Serialize(Stream stream, Entity? rawDifference)
        {
            base.Serialize(stream, rawDifference);
            EntityShulker? difference = rawDifference is EntityShulker castDifference ? castDifference : null;
            if (difference is not null ? difference.Face != Face : true)
            {
                stream.WriteU8(16);
                stream.WriteU8(MetadataType.Face);
                stream.WriteU8((byte)Face);
            }
            if (difference is not null ? difference.Peek != Peek : true)
            {
                stream.WriteU8(17);
                stream.WriteU8(MetadataType.Byte);
                stream.WriteU8(Peek);
            }
            if (difference is not null ? difference.Color != Color : true)
            {
                stream.WriteU8(18);
                stream.WriteU8(MetadataType.Byte);
                stream.WriteU8((byte)(Color.HasValue ? (int)Color.Value : 16));
            }
        }
        public override void CloneFrom(Entity rawEntity)
        {
            base.CloneFrom(rawEntity);
            if (rawEntity is not EntityShulker entity) return;
            Face = entity.Face;
            Peek = entity.Peek;
            Color = entity.Color;
        }
        public override EntityShulker Clone()
        {
            EntityShulker entity = new();
            entity.CloneFrom(this);
            return entity;
        }
    }
}
