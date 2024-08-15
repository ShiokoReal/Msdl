using System.IO;
using Net.Myzuc.PurpleStainedGlass.Protocol.Blocks;
using Net.Myzuc.PurpleStainedGlass.Protocol.Data.Blocks;
using Net.Myzuc.PurpleStainedGlass.Protocol.Const;
using Net.Myzuc.ShioLib;

namespace Net.Myzuc.PurpleStainedGlass.Protocol.Entities
{
    public sealed class EntityTnt : Entity
    {
        internal override int Id => 106;
        public override double HitboxHeight => 0.98;
        public override double HitboxWidth => 0.98;
        public override bool HitboxSoftCollision => false;
        public override bool HitboxHardCollision => false;
        public override bool HitboxAlign => false;
        public int Fuse = 80;
        public Block Block = new BlockTnt();
        public EntityTnt()
        {

        }
        internal override void Serialize(Stream stream, Entity? rawDifference)
        {
            base.Serialize(stream, rawDifference);
            EntityTnt? difference = rawDifference is EntityTnt castDifference ? castDifference : null;
            if (difference is not null ? difference.Fuse != Fuse : true)
            {
                stream.WriteU8(8);
                stream.WriteU8(MetadataType.S32V);
                stream.WriteS32V(Fuse);
            }
            if (difference is not null ? difference.Block != Block : true)
            {
                stream.WriteU8(9);
                stream.WriteU8(MetadataType.BlockId);
                stream.WriteS32V(Block.Id);
            }
        }
        public override void CloneFrom(Entity rawEntity)
        {
            base.CloneFrom(rawEntity);
            if (rawEntity is not EntityTnt entity) return;
            Fuse = entity.Fuse;
            Block = entity.Block.Clone();
        }
        public override EntityTnt Clone()
        {
            EntityTnt entity = new();
            entity.CloneFrom(this);
            return entity;
        }
    }
}
