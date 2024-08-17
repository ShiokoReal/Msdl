namespace Net.Myzuc.PurpleStainedGlass.Protocol.Blocks
{
    public sealed class BlockStructureBlock : Block
    {
        public enum EnumMode : int
        {
            Save = 0,
            Load = 1,
            Corner = 2,
            Data = 3
        }
        public override int BlockId => 19356 + (int)Mode * 1;
        public override int LiquidId => 0;
        public override int LightEmission => 0;
        public override int LightFilter => 15;
        public override (double xa, double ya, double za, double xb, double yb, double zb)[] Collisions => [
            (0, 0, 0, 1, 1, 1)
        ];
        public EnumMode Mode = EnumMode.Save;
        public BlockStructureBlock()
        {
            
        }
        public override BlockStructureBlock Clone()
        {
            return new()
            {
                Mode = Mode
            };
        }
        public override Block Break()
        {
            return new BlockAir();
        }
    }
}
