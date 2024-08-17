namespace Net.Myzuc.PurpleStainedGlass.Protocol.Blocks
{
    public sealed class BlockEndPortalFrame : Block
    {
        public enum EnumFacing : int
        {
            North = 0,
            South = 1,
            West = 2,
            East = 3
        }
        private static (double xa, double ya, double za, double xb, double yb, double zb)[][] InternalCollisions = [
            [
                (0, 0, 0, 1, 0.8125, 1),
                (0.25, 0.8125, 0.25, 0.75, 1, 0.75)
            ],
            [
                (0, 0, 0, 1, 0.8125, 1),
                (0.25, 0.8125, 0.25, 0.75, 1, 0.75)
            ],
            [
                (0, 0, 0, 1, 0.8125, 1),
                (0.25, 0.8125, 0.25, 0.75, 1, 0.75)
            ],
            [
                (0, 0, 0, 1, 0.8125, 1),
                (0.25, 0.8125, 0.25, 0.75, 1, 0.75)
            ],
            [
                (0, 0, 0, 1, 0.8125, 1)
            ],
            [
                (0, 0, 0, 1, 0.8125, 1)
            ],
            [
                (0, 0, 0, 1, 0.8125, 1)
            ],
            [
                (0, 0, 0, 1, 0.8125, 1)
            ]
        ];
        public override int BlockId => 7407 + (int)Facing * 1 + (Eye ? 0 : 4);
        public override int LiquidId => 0;
        public override int LightEmission => 1;
        public override int LightFilter => 0;
        public override (double xa, double ya, double za, double xb, double yb, double zb)[] Collisions => [.. InternalCollisions[BlockId - 7407]];
        public EnumFacing Facing = EnumFacing.North;
        public bool Eye = false;
        public BlockEndPortalFrame()
        {
            
        }
        public override BlockEndPortalFrame Clone()
        {
            return new()
            {
                Facing = Facing,
                Eye = Eye
            };
        }
        public override Block Break()
        {
            return new BlockAir();
        }
    }
}
