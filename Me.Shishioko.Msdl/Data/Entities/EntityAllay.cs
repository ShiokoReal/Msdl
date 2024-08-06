using System.IO;
using Net.Myzuc.ShioLib;

namespace Me.Shishioko.Msdl.Data.Entities
{
    public sealed class EntityAllay : EntityMob
    {
        public override int Id => 0;
        public override double Height => 0.6;
        public override double Width => 0.35;
        public bool Dancing = false;
        public bool Breedable = false;
        public EntityAllay()
        {

        }
        internal override void Serialize(Stream stream, EntityBase? rawDifference)
        {
            base.Serialize(stream, rawDifference);
            EntityAllay? difference = rawDifference is EntityAllay castDifference ? castDifference : null;
            if (difference is not null ? difference.Dancing != Dancing : true)
            {
                stream.WriteU8(16);
                stream.WriteU8(8);
                stream.WriteBool(Dancing);
            }
            if (difference is not null ? difference.Breedable != Breedable : true)
            {
                stream.WriteU8(16);
                stream.WriteU8(8);
                stream.WriteBool(Breedable);
            }
        }
    }
}
