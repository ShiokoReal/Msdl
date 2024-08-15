namespace Net.Myzuc.PurpleStainedGlass.Protocol.Enums
{
    public enum EnumDownloadFeedback : int
    {
        DownloadSuccess = 0,
        PromptDecline = 1,
        DownloadFailure = 2,
        PromptAccept = 3,
        DownloadComplete = 4,
        Misformat = 5,
        LoadFailure = 6,
        Discarded = 7,
    }
}
