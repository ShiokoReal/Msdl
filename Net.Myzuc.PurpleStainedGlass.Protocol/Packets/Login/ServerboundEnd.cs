namespace Net.Myzuc.PurpleStainedGlass.Protocol.Packets.Login
{
    public sealed class ServerboundEnd : Serverbound
    {
        internal ServerboundEnd(Client client)
        {
            client.State = Client.EnumState.Configuration;
        }
    }
}
