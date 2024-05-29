namespace Me.Shishioko.Msdl.Data.Types
{
    public abstract class BlockTypeStone : Block
    {
        internal BlockTypeStone()
        {

        }
        public sealed override float GetHardness() => 1.5f;
        public sealed override Material GetMaterial() => Material.PickaxeMineable;
        public sealed override byte GetLightEmissiom() => 0;
        public sealed override byte GetLightFilter() => 15;
        public sealed override BoundingBox GetBoundingBox() => BoundingBox.Block;
    }
}
