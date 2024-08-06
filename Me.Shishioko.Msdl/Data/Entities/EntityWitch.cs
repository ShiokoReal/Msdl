using System.IO;
using Net.Myzuc.ShioLib;

namespace Me.Shishioko.Msdl.Data.Entities
{
    public sealed class EntityWitch : EntityMob
    {
        public override int Id => 118;
        public override double Height => 1.95;
        public override double Width => 0.6;
        public bool Celebrating = false;
        public bool Drinking = false;
        public EntityWitch()
        {

        }
        internal override void Serialize(Stream stream, EntityBase? rawDifference)
        {
            base.Serialize(stream, rawDifference);
            EntityWitch? difference = rawDifference is EntityWitch castDifference ? castDifference : null;
            if (difference is not null ? difference.Celebrating != Celebrating : true)
            {
                stream.WriteU8(16);
                stream.WriteU8(8);
                stream.WriteBool(Celebrating);
            }
            if (difference is not null ? difference.Drinking != Drinking : true)
            {
                stream.WriteU8(17);
                stream.WriteU8(8);
                stream.WriteBool(Drinking);
            }
        }
    }
}
