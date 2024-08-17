using System.ComponentModel.DataAnnotations;
namespace Net.Myzuc.PurpleStainedGlass.Protocol.Blocks
{
    public sealed class BlockNoteBlock : Block
    {
        public enum EnumInstrument : int
        {
            Harp = 0,
            Basedrum = 1,
            Snare = 2,
            Hat = 3,
            Bass = 4,
            Flute = 5,
            Bell = 6,
            Guitar = 7,
            Chime = 8,
            Xylophone = 9,
            IronXylophone = 10,
            CowBell = 11,
            Didgeridoo = 12,
            Bit = 13,
            Banjo = 14,
            Pling = 15,
            Zombie = 16,
            Skeleton = 17,
            Creeper = 18,
            Dragon = 19,
            WitherSkeleton = 20,
            Piglin = 21,
            CustomHead = 22
        }
        public override int BlockId => 538 + (Powered ? 0 : 1) + Note * 2 + (int)Instrument * 50;
        public override int LiquidId => 0;
        public override int LightEmission => 0;
        public override int LightFilter => 15;
        public override (double xa, double ya, double za, double xb, double yb, double zb)[] Collisions => [
            (0, 0, 0, 1, 1, 1)
        ];
        public bool Powered = false;
        [Range(0, 24)]
        public int Note = 0;
        public EnumInstrument Instrument = EnumInstrument.Harp;
        public BlockNoteBlock()
        {
            
        }
        public override BlockNoteBlock Clone()
        {
            return new()
            {
                Powered = Powered,
                Note = Note,
                Instrument = Instrument
            };
        }
        public override Block Break()
        {
            return new BlockAir();
        }
    }
}
