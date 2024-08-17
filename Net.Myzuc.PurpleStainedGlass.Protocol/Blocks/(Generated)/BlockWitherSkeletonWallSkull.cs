namespace Net.Myzuc.PurpleStainedGlass.Protocol.Blocks
{
    public sealed class BlockWitherSkeletonWallSkull : Block
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
                (0.25, 0.25, 0.5, 0.75, 0.75, 1)
            ],
            [
                (0.25, 0.25, 0.5, 0.75, 0.75, 1)
            ],
            [
                (0.25, 0.25, 0, 0.75, 0.75, 0.5)
            ],
            [
                (0.25, 0.25, 0, 0.75, 0.75, 0.5)
            ],
            [
                (0.5, 0.25, 0.25, 1, 0.75, 0.75)
            ],
            [
                (0.5, 0.25, 0.25, 1, 0.75, 0.75)
            ],
            [
                (0, 0.25, 0.25, 0.5, 0.75, 0.75)
            ],
            [
                (0, 0.25, 0.25, 0.5, 0.75, 0.75)
            ]
        ];
        public override int BlockId => 8899 + (Powered ? 0 : 1) + (int)Facing * 2;
        public override int LiquidId => 0;
        public override int LightEmission => 0;
        public override int LightFilter => 0;
        public override (double xa, double ya, double za, double xb, double yb, double zb)[] Collisions => [.. InternalCollisions[BlockId - 8899]];
        public bool Powered = false;
        public EnumFacing Facing = EnumFacing.North;
        public BlockWitherSkeletonWallSkull()
        {
            
        }
        public override BlockWitherSkeletonWallSkull Clone()
        {
            return new()
            {
                Powered = Powered,
                Facing = Facing
            };
        }
        public override Block Break()
        {
            return new BlockAir();
        }
    }
}
