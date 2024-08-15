namespace Net.Myzuc.PurpleStainedGlass.Protocol.Packets.Configuration
{
    public sealed class ServerboundEnd : Serverbound
    {
        internal ServerboundEnd(Client client)
        {
            client.State = Client.EnumState.Play;
        }
    }
}
