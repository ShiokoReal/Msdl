using System.IO;

namespace Me.Shishioko.Msdl.Data.Entities
{
    public sealed class EntityZoglin : EntityGrowable
    {
        public override int Id => 123;
        public override double Height => 1.4;
        public override double Width => 1.3964844;
        public EntityZoglin()
        {

        }
        internal override void Serialize(Stream stream, EntityBase? rawDifference)
        {
            base.Serialize(stream, rawDifference);
        }
    }
}
