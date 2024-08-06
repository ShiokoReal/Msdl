using System.IO;
using Me.Shishioko.Msdl.Data.Protocol;
using Net.Myzuc.ShioLib;

namespace Me.Shishioko.Msdl.Data.Entities
{
    public sealed class EntityDragon : EntityMob
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
        public EntityDragon()
        {

        }
        internal override void Serialize(Stream stream, EntityBase? rawDifference)
        {
            base.Serialize(stream, rawDifference);
            EntityDragon? difference = rawDifference is EntityDragon castDifference ? castDifference : null;
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
            if (rawEntity is not EntityDragon entity) return;
            Phase = entity.Phase;
        }
    }
}
