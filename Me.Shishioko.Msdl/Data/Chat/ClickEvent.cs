namespace Me.Shishioko.Msdl.Data.Chat
{
    public abstract class ClickEvent
    {
        public sealed class Url : ClickEvent
        {
            internal override string Type => "open_url";
            public Url(string url) : base(url)
            {

            }
        }
        public sealed class Send : ClickEvent
        {
            internal override string Type => "run_command";
            public Send(string text) : base(text)
            {

            }
        }
        public sealed class Paste : ClickEvent
        {
            internal override string Type => "suggest_command";
            public Paste(string text) : base(text)
            {

            }
        }
        public sealed class Page : ClickEvent
        {
            internal override string Type => "suggest_command";
            public Page(int page) : base(page.ToString())
            {

            }
        }
        public sealed class Copy : ClickEvent
        {
            internal override string Type => "copy_to_clipboard";
            public Copy(string text) : base(text)
            {

            }
        }
        internal abstract string Type { get; }
        public string Content; 
        private ClickEvent(string content)
        {
            Content = content;
        }
    }
}
