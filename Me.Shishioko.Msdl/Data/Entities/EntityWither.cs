using System.IO;
using Me.Shishioko.Msdl.Data.Protocol;
using Net.Myzuc.ShioLib;

namespace Me.Shishioko.Msdl.Data.Entities
{
    public sealed class EntityWither : EntityMob
    {
        internal override int Id => 119;
        public override double Height => 3.5;
        public override double Width => 0.9;
        public int CenterTargetEID = 0;
        public int LeftTargetEID = 0;
        public int RightTargetEID = 0;
        public int Invulnerability = 0;
        public EntityWither()
        {

        }
        internal override void Serialize(Stream stream, EntityBase? rawDifference)
        {
            base.Serialize(stream, rawDifference);
            EntityWither? difference = rawDifference is EntityWither castDifference ? castDifference : null;
            if (difference is not null ? difference.CenterTargetEID != CenterTargetEID : true)
            {
                stream.WriteU8(16);
                stream.WriteS32V(MetadataType.S32V);
                stream.WriteS32V(CenterTargetEID);
            }
            if (difference is not null ? difference.LeftTargetEID != LeftTargetEID : true)
            {
                stream.WriteU8(17);
                stream.WriteS32V(MetadataType.S32V);
                stream.WriteS32V(LeftTargetEID);
            }
            if (difference is not null ? difference.RightTargetEID != RightTargetEID : true)
            {
                stream.WriteU8(18);
                stream.WriteS32V(MetadataType.S32V);
                stream.WriteS32V(RightTargetEID);
            }
            if (difference is not null ? difference.Invulnerability != Invulnerability : true)
            {
                stream.WriteU8(19);
                stream.WriteS32V(MetadataType.S32V);
                stream.WriteS32V(Invulnerability);
            }
        }
        public override void Clone(EntityBase rawEntity)
        {
            base.Clone(rawEntity);
            if (rawEntity is not EntityWither entity) return;
            CenterTargetEID = entity.CenterTargetEID;
            LeftTargetEID = entity.LeftTargetEID;
            RightTargetEID = entity.RightTargetEID;
            Invulnerability = entity.Invulnerability;
        }
    }
}
