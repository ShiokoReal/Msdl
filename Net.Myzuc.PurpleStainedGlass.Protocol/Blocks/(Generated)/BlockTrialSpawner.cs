namespace Net.Myzuc.PurpleStainedGlass.Protocol.Blocks
{
    public sealed class BlockTrialSpawner : Block
    {
        public enum EnumTrialSpawnerState : int
        {
            Inactive = 0,
            WaitingForPlayers = 1,
            Active = 2,
            WaitingForRewardEjection = 3,
            EjectingReward = 4,
            Cooldown = 5
        }
        public override int BlockId => 26638 + (int)TrialSpawnerState * 1 + (Ominous ? 0 : 6);
        public override int LiquidId => 0;
        public override int LightEmission => 0;
        public override int LightFilter => 1;
        public override (double xa, double ya, double za, double xb, double yb, double zb)[] Collisions => [
            (0, 0, 0, 1, 1, 1)
        ];
        public EnumTrialSpawnerState TrialSpawnerState = EnumTrialSpawnerState.Inactive;
        public bool Ominous = false;
        public BlockTrialSpawner()
        {
            
        }
        public override BlockTrialSpawner Clone()
        {
            return new()
            {
                TrialSpawnerState = TrialSpawnerState,
                Ominous = Ominous
            };
        }
        public override Block Break()
        {
            return new BlockAir();
        }
    }
}
