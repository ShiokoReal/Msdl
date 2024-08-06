using System.IO;
using Net.Myzuc.ShioLib;

namespace Me.Shishioko.Msdl.Data.Entities
{
    public sealed class EntityLightning : EntityBase
    {
        public override int Id => 64;
        public override double Height => 0.0;
        public override double Width => 0.0;
        public EntityLightning()
        {

        }
        internal override void Serialize(Stream stream, EntityBase? rawDifference)
        {
            base.Serialize(stream, rawDifference);
        }
    }
}
