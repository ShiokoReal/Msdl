using System.ComponentModel.DataAnnotations;
namespace Net.Myzuc.PurpleStainedGlass.Protocol.Blocks
{
    public sealed class BlockCalibratedSculkSensor : Block
    {
        public enum EnumSculkSensorPhase : int
        {
            Inactive = 0,
            Active = 1,
            Cooldown = 2
        }
        public enum EnumFacing : int
        {
            North = 0,
            South = 1,
            West = 2,
            East = 3
        }
        public override int BlockId => 22415 + (Waterlogged ? 0 : 1) + (int)SculkSensorPhase * 2 + Power * 6 + (int)Facing * 96;
        public override int LiquidId => Waterlogged ? 1 : 0;
        public override int LightEmission => 1;
        public override int LightFilter => 0;
        public override (double xa, double ya, double za, double xb, double yb, double zb)[] Collisions => [
            (0, 0, 0, 1, 0.5, 1)
        ];
        public bool Waterlogged = false;
        public EnumSculkSensorPhase SculkSensorPhase = EnumSculkSensorPhase.Inactive;
        [Range(0, 15)]
        public int Power = 0;
        public EnumFacing Facing = EnumFacing.North;
        public BlockCalibratedSculkSensor()
        {
            
        }
        public override BlockCalibratedSculkSensor Clone()
        {
            return new()
            {
                Waterlogged = Waterlogged,
                SculkSensorPhase = SculkSensorPhase,
                Power = Power,
                Facing = Facing
            };
        }
        public override Block Break()
        {
            return Waterlogged ? new BlockWater() : new BlockAir();
        }
    }
}
