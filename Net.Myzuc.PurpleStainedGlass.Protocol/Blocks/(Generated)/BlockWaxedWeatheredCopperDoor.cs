namespace Net.Myzuc.PurpleStainedGlass.Protocol.Blocks
{
    public sealed class BlockWaxedWeatheredCopperDoor : Block
    {
        public enum EnumHinge : int
        {
            Left = 0,
            Right = 1
        }
        public enum EnumHalf : int
        {
            Upper = 0,
            Lower = 1
        }
        public enum EnumFacing : int
        {
            North = 0,
            South = 1,
            West = 2,
            East = 3
        }
        private static (double xa, double ya, double za, double xb, double yb, double zb)[][] InternalCollisions = [
            [
                (0, 0, 0, 0.1875, 1, 1)
            ],
            [
                (0, 0, 0, 0.1875, 1, 1)
            ],
            [
                (0, 0, 0.8125, 1, 1, 1)
            ],
            [
                (0, 0, 0.8125, 1, 1, 1)
            ],
            [
                (0.8125, 0, 0, 1, 1, 1)
            ],
            [
                (0.8125, 0, 0, 1, 1, 1)
            ],
            [
                (0, 0, 0.8125, 1, 1, 1)
            ],
            [
                (0, 0, 0.8125, 1, 1, 1)
            ],
            [
                (0, 0, 0, 0.1875, 1, 1)
            ],
            [
                (0, 0, 0, 0.1875, 1, 1)
            ],
            [
                (0, 0, 0.8125, 1, 1, 1)
            ],
            [
                (0, 0, 0.8125, 1, 1, 1)
            ],
            [
                (0.8125, 0, 0, 1, 1, 1)
            ],
            [
                (0.8125, 0, 0, 1, 1, 1)
            ],
            [
                (0, 0, 0.8125, 1, 1, 1)
            ],
            [
                (0, 0, 0.8125, 1, 1, 1)
            ],
            [
                (0.8125, 0, 0, 1, 1, 1)
            ],
            [
                (0.8125, 0, 0, 1, 1, 1)
            ],
            [
                (0, 0, 0, 1, 1, 0.1875)
            ],
            [
                (0, 0, 0, 1, 1, 0.1875)
            ],
            [
                (0, 0, 0, 0.1875, 1, 1)
            ],
            [
                (0, 0, 0, 0.1875, 1, 1)
            ],
            [
                (0, 0, 0, 1, 1, 0.1875)
            ],
            [
                (0, 0, 0, 1, 1, 0.1875)
            ],
            [
                (0.8125, 0, 0, 1, 1, 1)
            ],
            [
                (0.8125, 0, 0, 1, 1, 1)
            ],
            [
                (0, 0, 0, 1, 1, 0.1875)
            ],
            [
                (0, 0, 0, 1, 1, 0.1875)
            ],
            [
                (0, 0, 0, 0.1875, 1, 1)
            ],
            [
                (0, 0, 0, 0.1875, 1, 1)
            ],
            [
                (0, 0, 0, 1, 1, 0.1875)
            ],
            [
                (0, 0, 0, 1, 1, 0.1875)
            ],
            [
                (0, 0, 0.8125, 1, 1, 1)
            ],
            [
                (0, 0, 0.8125, 1, 1, 1)
            ],
            [
                (0.8125, 0, 0, 1, 1, 1)
            ],
            [
                (0.8125, 0, 0, 1, 1, 1)
            ],
            [
                (0, 0, 0, 1, 1, 0.1875)
            ],
            [
                (0, 0, 0, 1, 1, 0.1875)
            ],
            [
                (0.8125, 0, 0, 1, 1, 1)
            ],
            [
                (0.8125, 0, 0, 1, 1, 1)
            ],
            [
                (0, 0, 0.8125, 1, 1, 1)
            ],
            [
                (0, 0, 0.8125, 1, 1, 1)
            ],
            [
                (0.8125, 0, 0, 1, 1, 1)
            ],
            [
                (0.8125, 0, 0, 1, 1, 1)
            ],
            [
                (0, 0, 0, 1, 1, 0.1875)
            ],
            [
                (0, 0, 0, 1, 1, 0.1875)
            ],
            [
                (0.8125, 0, 0, 1, 1, 1)
            ],
            [
                (0.8125, 0, 0, 1, 1, 1)
            ],
            [
                (0, 0, 0, 1, 1, 0.1875)
            ],
            [
                (0, 0, 0, 1, 1, 0.1875)
            ],
            [
                (0, 0, 0, 0.1875, 1, 1)
            ],
            [
                (0, 0, 0, 0.1875, 1, 1)
            ],
            [
                (0, 0, 0.8125, 1, 1, 1)
            ],
            [
                (0, 0, 0.8125, 1, 1, 1)
            ],
            [
                (0, 0, 0, 0.1875, 1, 1)
            ],
            [
                (0, 0, 0, 0.1875, 1, 1)
            ],
            [
                (0, 0, 0, 1, 1, 0.1875)
            ],
            [
                (0, 0, 0, 1, 1, 0.1875)
            ],
            [
                (0, 0, 0, 0.1875, 1, 1)
            ],
            [
                (0, 0, 0, 0.1875, 1, 1)
            ],
            [
                (0, 0, 0.8125, 1, 1, 1)
            ],
            [
                (0, 0, 0.8125, 1, 1, 1)
            ],
            [
                (0, 0, 0, 0.1875, 1, 1)
            ],
            [
                (0, 0, 0, 0.1875, 1, 1)
            ]
        ];
        public override int BlockId => 24100 + (Powered ? 0 : 1) + (Open ? 0 : 2) + (int)Hinge * 4 + (int)Half * 8 + (int)Facing * 16;
        public override int LiquidId => 0;
        public override int LightEmission => 0;
        public override int LightFilter => 0;
        public override (double xa, double ya, double za, double xb, double yb, double zb)[] Collisions => [.. InternalCollisions[BlockId - 24100]];
        public bool Powered = false;
        public bool Open = false;
        public EnumHinge Hinge = EnumHinge.Left;
        public EnumHalf Half = EnumHalf.Upper;
        public EnumFacing Facing = EnumFacing.North;
        public BlockWaxedWeatheredCopperDoor()
        {
            
        }
        public override BlockWaxedWeatheredCopperDoor Clone()
        {
            return new()
            {
                Powered = Powered,
                Open = Open,
                Hinge = Hinge,
                Half = Half,
                Facing = Facing
            };
        }
        public override Block Break()
        {
            return new BlockAir();
        }
    }
}
