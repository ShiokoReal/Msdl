using System.IO;
using Me.Shishioko.Msdl.Data.Protocol;
using Net.Myzuc.ShioLib;

namespace Me.Shishioko.Msdl.Data.Entities
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
        public int Block = 2094; //TODO: tnt block id from block classes read to int
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
                stream.WriteS32V(Block);
            }
        }
        public override void Clone(Entity rawEntity)
        {
            base.Clone(rawEntity);
            if (rawEntity is not EntityTnt entity) return;
            Fuse = entity.Fuse;
            Block = entity.Block;
        }
    }
}
