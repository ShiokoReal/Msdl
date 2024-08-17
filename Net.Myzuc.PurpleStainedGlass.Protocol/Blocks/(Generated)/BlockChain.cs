namespace Net.Myzuc.PurpleStainedGlass.Protocol.Blocks
{
    public sealed class BlockChain : Block
    {
        public enum EnumAxis : int
        {
            X = 0,
            Y = 1,
            Z = 2
        }
        private static (double xa, double ya, double za, double xb, double yb, double zb)[][] InternalCollisions = [
            [
                (0, 0.40625, 0.40625, 1, 0.59375, 0.59375)
            ],
            [
                (0, 0.40625, 0.40625, 1, 0.59375, 0.59375)
            ],
            [
                (0.40625, 0, 0.40625, 0.59375, 1, 0.59375)
            ],
            [
                (0.40625, 0, 0.40625, 0.59375, 1, 0.59375)
            ],
            [
                (0.40625, 0.40625, 0, 0.59375, 0.59375, 1)
            ],
            [
                (0.40625, 0.40625, 0, 0.59375, 0.59375, 1)
            ]
        ];
        public override int BlockId => 6773 + (Waterlogged ? 0 : 1) + (int)Axis * 2;
        public override int LiquidId => Waterlogged ? 1 : 0;
        public override int LightEmission => 0;
        public override int LightFilter => 0;
        public override (double xa, double ya, double za, double xb, double yb, double zb)[] Collisions => [.. InternalCollisions[BlockId - 6773]];
        public bool Waterlogged = false;
        public EnumAxis Axis = EnumAxis.X;
        public BlockChain()
        {
            
        }
        public override BlockChain Clone()
        {
            return new()
            {
                Waterlogged = Waterlogged,
                Axis = Axis
            };
        }
        public override Block Break()
        {
            return Waterlogged ? new BlockWater() : new BlockAir();
        }
    }
}
