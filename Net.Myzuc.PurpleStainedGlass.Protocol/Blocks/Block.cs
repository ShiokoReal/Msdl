namespace Net.Myzuc.PurpleStainedGlass.Protocol.Blocks
{
    public abstract class Block
    {
        public abstract int BlockId { get; }
        public abstract int LiquidId { get; }
        public abstract int LightEmission { get; }
        public abstract int LightFilter { get; }
        public abstract (double xa, double ya, double za, double xb, double yb, double zb)[] Collisions { get; }
        internal Block()
        {

        }
        public abstract Block Clone();
        public abstract Block Break();
    }
}
