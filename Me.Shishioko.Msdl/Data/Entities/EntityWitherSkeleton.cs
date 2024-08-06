using System.IO;

namespace Me.Shishioko.Msdl.Data.Entities
{
    public sealed class EntityWitherSkeleton : EntityMob
    {
        public override int Id => 120;
        public override double Height => 2.4;
        public override double Width => 0.7;
        public EntityWitherSkeleton()
        {

        }
        internal override void Serialize(Stream stream, EntityBase? rawDifference)
        {
            base.Serialize(stream, rawDifference);
        }
    }
}
