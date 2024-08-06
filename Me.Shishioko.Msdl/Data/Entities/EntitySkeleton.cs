using System.IO;
using Net.Myzuc.ShioLib;

namespace Me.Shishioko.Msdl.Data.Entities
{
    public sealed class EntitySkeleton : EntityMob
    {
        public override int Id => 91;
        public override double Height => 1.99;
        public override double Width => 0.6;
        public bool Transforming = false;
        public EntitySkeleton()
        {

        }
        internal override void Serialize(Stream stream, EntityBase? rawDifference)
        {
            base.Serialize(stream, rawDifference);
            EntitySkeleton? difference = rawDifference is EntitySkeleton castDifference ? castDifference : null;
            if (difference is not null ? difference.Transforming != Transforming : true)
            {
                stream.WriteU8(16);
                stream.WriteU8(8);
                stream.WriteBool(Transforming);
            }
        }
    }
}
