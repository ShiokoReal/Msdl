using System.IO;
using Net.Myzuc.ShioLib;

namespace Me.Shishioko.Msdl.Data.Entities
{
    public sealed class EntitySnowGolem : EntityMob
    {
        public override int Id => 96;
        public override double Height => 1.9;
        public override double Width => 0.7;
        public bool Pumpkin = false;
        public EntitySnowGolem()
        {

        }
        internal override void Serialize(Stream stream, EntityBase? rawDifference)
        {
            base.Serialize(stream, rawDifference);
            EntitySnowGolem? difference = rawDifference is EntitySnowGolem castDifference ? castDifference : null;
            if (difference is not null ? difference.Pumpkin != Pumpkin : true)
            {
                stream.WriteU8(16);
                stream.WriteU8(0);
                stream.WriteU8((byte)(Pumpkin ? 0x10 : 0x00));
            }
        }
    }
}
