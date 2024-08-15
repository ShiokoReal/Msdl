namespace Net.Myzuc.PurpleStainedGlass.Protocol.Objects.Chat
{
    public sealed class ClickEvent
    {
        public static string[] Types => ["copy_to_clipboard", "suggest_command", "change_page", "run_command", "open_url"];
        public string Type { get; }
        public string Content { get; }
        private ClickEvent(string type, string content)
        {
            Type = type;
            Content = content;
        }
    }
}
