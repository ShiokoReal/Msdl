using System;
using System.IO;
using Me.Shishioko.Msdl.Data.Protocol;
using Net.Myzuc.ShioLib;

namespace Me.Shishioko.Msdl.Data.Entities
{
    public sealed class EntityTnt : EntityBase
    {
        internal override int Id => 106;
        public override double Height => 0.98;
        public override double Width => 0.98;
        public int Fuse = 80;
        public int Block = 2094; //TODO: tnt block id from block classes read to int
        public EntityTnt()
        {

        }
        internal override void Serialize(Stream stream, EntityBase? rawDifference)
        {
            base.Serialize(stream, rawDifference);
            EntityTnt? difference = rawDifference is EntityTnt castDifference ? castDifference : null;
            if (difference is not null ? difference.Fuse != Fuse : true)
            {
                stream.WriteU8(8);
                stream.WriteS32V(MetadataType.S32V);
                stream.WriteS32V(Fuse);
            }
            if (difference is not null ? difference.Block != Block : true)
            {
                stream.WriteU8(9);
                stream.WriteS32V(MetadataType.BlockId);
                stream.WriteS32V(Block);
            }
        }
        public override void Clone(EntityBase rawEntity)
        {
            base.Clone(rawEntity);
            if (rawEntity is not EntityTnt entity) return;
            Fuse = entity.Fuse;
            Block = entity.Block;
        }
    }
}
