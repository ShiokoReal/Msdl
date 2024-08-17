namespace Net.Myzuc.PurpleStainedGlass.Protocol.Blocks
{
    public sealed class BlockVault : Block
    {
        public enum EnumVaultState : int
        {
            Inactive = 0,
            Active = 1,
            Unlocking = 2,
            Ejecting = 3
        }
        public enum EnumFacing : int
        {
            North = 0,
            South = 1,
            West = 2,
            East = 3
        }
        public override int BlockId => 26650 + (int)VaultState * 1 + (Ominous ? 0 : 4) + (int)Facing * 8;
        public override int LiquidId => 0;
        public override int LightEmission => 6;
        public override int LightFilter => 1;
        public override (double xa, double ya, double za, double xb, double yb, double zb)[] Collisions => [
            (0, 0, 0, 1, 1, 1)
        ];
        public EnumVaultState VaultState = EnumVaultState.Inactive;
        public bool Ominous = false;
        public EnumFacing Facing = EnumFacing.North;
        public BlockVault()
        {
            
        }
        public override BlockVault Clone()
        {
            return new()
            {
                VaultState = VaultState,
                Ominous = Ominous,
                Facing = Facing
            };
        }
        public override Block Break()
        {
            return new BlockAir();
        }
    }
}
