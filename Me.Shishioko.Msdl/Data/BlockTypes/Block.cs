using Me.Shishioko.Msdl.Data.Blocks;

namespace Me.Shishioko.Msdl.Data.Types
{
    public abstract class Block
    {
        internal static Block FromProtocolID(ushort id)
        {
            return id switch
            {
                0 => new BlockAir(),
                1 => new BlockStone(),
                2 => new BlockGranite(),
                3 => new BlockPolishedGranite(),
                4 => new BlockDiorite(),
                5 => new BlockPolishedDiorite(),
                6 => new BlockAndesite(),
                7 => new BlockPolishedAndesite(),
                _ => new BlockAir(),
            };
        }
        internal Block()
        {

        }
        internal abstract ushort GetProtocolID();
        public abstract string GetName();
        public abstract float GetHardness();
        public abstract Material GetMaterial();
        public abstract byte GetLightEmissiom();
        public abstract byte GetLightFilter();
        public abstract BoundingBox GetBoundingBox();
        public override int GetHashCode()
        {
            return GetProtocolID();
        }
    }
}
