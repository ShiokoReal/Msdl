using System.IO;
using Net.Myzuc.ShioLib;

namespace Me.Shishioko.Msdl.Data.Entities
{
    public sealed class EntityWarden : EntityMob
    {
        public override int Id => 116;
        public override double Height => 2.9;
        public override double Width => 0.9;
        public int Anger = 0;
        public EntityWarden()
        {

        }
        internal override void Serialize(Stream stream, EntityBase? rawDifference)
        {
            base.Serialize(stream, rawDifference);
            EntityWarden? difference = rawDifference is EntityWarden castDifference ? castDifference : null;
            if (difference is not null ? difference.Anger != Anger : true)
            {
                stream.WriteU8(16);
                stream.WriteU8(1);
                stream.WriteS32V(Anger);
            }
        }
    }
}
