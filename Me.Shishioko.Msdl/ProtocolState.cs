namespace Me.Shishioko.Msdl
{
    public enum ProtocolState : int
    {
        Disconnected = -1,
        Handshake = 0,
        Status = 1,
        Login = 2,
        Configuration = 3,
        Play = 4,
        PlayToConfiguration = 5, //TODO: same for login to config
    }
}
