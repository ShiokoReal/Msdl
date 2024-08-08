using System.IO;
using Me.Shishioko.Msdl.Data.Protocol;
using Net.Myzuc.ShioLib;

namespace Me.Shishioko.Msdl.Data.Entities
{
    public sealed class EntityWither : EntityMob
    {
        internal override int Id => 119;
        public override double HitboxHeight => 3.5;
        public override double HitboxWidth => 0.9;
        public override bool HitboxSoftCollision => true;
        public override bool HitboxHardCollision => false;
        public override bool HitboxAlign => false;
        public int CenterTargetEID = 0;
        public int LeftTargetEID = 0;
        public int RightTargetEID = 0;
        public int Invulnerability = 0;
        public EntityWither()
        {

        }
        internal override void Serialize(Stream stream, Entity? rawDifference)
        {
            base.Serialize(stream, rawDifference);
            EntityWither? difference = rawDifference is EntityWither castDifference ? castDifference : null;
            if (difference is not null ? difference.CenterTargetEID != CenterTargetEID : true)
            {
                stream.WriteU8(16);
                stream.WriteU8(MetadataType.S32V);
                stream.WriteS32V(CenterTargetEID);
            }
            if (difference is not null ? difference.LeftTargetEID != LeftTargetEID : true)
            {
                stream.WriteU8(17);
                stream.WriteU8(MetadataType.S32V);
                stream.WriteS32V(LeftTargetEID);
            }
            if (difference is not null ? difference.RightTargetEID != RightTargetEID : true)
            {
                stream.WriteU8(18);
                stream.WriteU8(MetadataType.S32V);
                stream.WriteS32V(RightTargetEID);
            }
            if (difference is not null ? difference.Invulnerability != Invulnerability : true)
            {
                stream.WriteU8(19);
                stream.WriteU8(MetadataType.S32V);
                stream.WriteS32V(Invulnerability);
            }
        }
        public override void Clone(Entity rawEntity)
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
