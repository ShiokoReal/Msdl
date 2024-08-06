using System.IO;
using Net.Myzuc.ShioLib;

namespace Me.Shishioko.Msdl.Data.Entities
{
    public sealed class EntitySpider : EntityMob
    {
        public override int Id => 100;
        public override double Height => 0.9;
        public override double Width => 1.4;
        public bool Climbing = false;
        public EntitySpider()
        {

        }
        internal override void Serialize(Stream stream, EntityBase? rawDifference)
        {
            base.Serialize(stream, rawDifference);
            EntitySpider? difference = rawDifference is EntitySpider castDifference ? castDifference : null;
            if (difference is not null ? difference.Climbing != Climbing : true)
            {
                stream.WriteU8(16);
                stream.WriteU8(0);
                stream.WriteS32V(Climbing ? 0x01 : 0x00);
            }
        }
    }
}
