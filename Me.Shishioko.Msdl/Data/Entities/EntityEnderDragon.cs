using System.IO;
using Me.Shishioko.Msdl.Data.Protocol;
using Net.Myzuc.ShioLib;

namespace Me.Shishioko.Msdl.Data.Entities
{
    public sealed class EntityEnderDragon : EntityMob
    {
        public enum EntityEnderdragonPhase
        {
            Circling = 0,
            Strafing = 1,
            PreLanding = 2,
            Landing = 3,
            Ascending = 4,
            Attack = 5,
            AttackSearch = 6,
            AttackWarn = 7,
            Charging = 8,
            Dying = 9,
            Hovering = 10,
        }
        internal override int Id => 31;
        public override double HitboxHeight => 8.0;
        public override double HitboxWidth => 16.0;
        public override bool HitboxSoftCollision => false;
        public override bool HitboxHardCollision => false;
        public override bool HitboxAlign => false;
        public EntityEnderdragonPhase Phase = EntityEnderdragonPhase.Hovering;
        public EntityEnderDragon()
        {

        }
        internal override void Serialize(Stream stream, EntityBase? rawDifference)
        {
            base.Serialize(stream, rawDifference);
            EntityEnderDragon? difference = rawDifference is EntityEnderDragon castDifference ? castDifference : null;
            if (difference is not null ? difference.Phase != Phase : true)
            {
                stream.WriteU8(16);
                stream.WriteU8(MetadataType.S32V);
                stream.WriteS32V((int)Phase);
            }
        }
        public override void Clone(EntityBase rawEntity)
        {
            base.Clone(rawEntity);
            if (rawEntity is not EntityEnderDragon entity) return;
            Phase = entity.Phase;
        }
    }
}
