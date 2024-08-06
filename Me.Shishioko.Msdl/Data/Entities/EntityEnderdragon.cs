using System.IO;
using Net.Myzuc.ShioLib;

namespace Me.Shishioko.Msdl.Data.Entities
{
    public sealed class EntityEnderdragon : EntityMob
    {
        public enum DragonPhase
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
        public override int Id => 31;
        public override double Height => 8.0;
        public override double Width => 16.0;
        public DragonPhase Phase = DragonPhase.Hovering;
        public EntityEnderdragon()
        {

        }
        internal override void Serialize(Stream stream, EntityBase? rawDifference)
        {
            base.Serialize(stream, rawDifference);
            EntityEnderdragon? difference = rawDifference is EntityEnderdragon castDifference ? castDifference : null;
            if (difference is not null ? difference.Phase != Phase : true)
            {
                stream.WriteU8(16);
                stream.WriteU8(1);
                stream.WriteS32V((int)Phase);
            }
        }
    }
}
