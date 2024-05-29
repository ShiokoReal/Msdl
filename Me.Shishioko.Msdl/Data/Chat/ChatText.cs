using Newtonsoft.Json;

namespace Me.Shishioko.Msdl.Data.Chat
{
    public sealed class ChatText : ChatComponent
    {
        [JsonProperty("text")]
        public string Text { get; set; }
        public ChatText(string text)
        {
            Text = text;
        }
    }
}
