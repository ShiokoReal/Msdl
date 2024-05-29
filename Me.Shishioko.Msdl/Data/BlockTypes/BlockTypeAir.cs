namespace Me.Shishioko.Msdl.Data.Types
{
    public abstract class BlockTypeAir : Block
    {
        internal BlockTypeAir()
        {

        }
        public sealed override float GetHardness() => float.PositiveInfinity;
        public sealed override Material GetMaterial() => Material.Default;
        public sealed override byte GetLightEmissiom() => 0;
        public sealed override byte GetLightFilter() => 0;
        public sealed override BoundingBox GetBoundingBox() => BoundingBox.Empty;
    }
}
