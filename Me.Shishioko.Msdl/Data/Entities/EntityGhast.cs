using System.IO;
using Net.Myzuc.ShioLib;

namespace Me.Shishioko.Msdl.Data.Entities
{
    public sealed class EntityGhast : EntityMob
    {
        public override int Id => 45;
        public override double Height => 4;
        public override double Width => 4;
        public bool Charging = false;
        public EntityGhast()
        {

        }
        internal override void Serialize(Stream stream, EntityBase? rawDifference)
        {
            base.Serialize(stream, rawDifference);
            EntityGhast? difference = rawDifference is EntityGhast castDifference ? castDifference : null;
            if (difference is not null ? difference.Charging != Charging : true)
            {
                stream.WriteU8(16);
                stream.WriteU8(8);
                stream.WriteBool(Charging);
            }
        }
    }
}
