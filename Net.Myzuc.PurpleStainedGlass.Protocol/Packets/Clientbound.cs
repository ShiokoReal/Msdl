namespace Net.Myzuc.PurpleStainedGlass.Protocol.Packets
{
    public abstract class Clientbound
    {
        public abstract Client.EnumState Mode { get; }
        internal byte[] Buffer;
        internal Clientbound()
        {

        }
        internal abstract void Transform(Client client);
    }
}
