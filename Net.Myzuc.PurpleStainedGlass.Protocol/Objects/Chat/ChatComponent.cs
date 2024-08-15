using Net.Myzuc.PurpleStainedGlass.Protocol.Objects;
using Net.Myzuc.ShioLib;
using System.IO;
using System.Text;
using System.Web;

namespace Net.Myzuc.PurpleStainedGlass.Protocol.Objects.Chat
{
    public abstract class ChatComponent
    {
        public Styling? Styling { get; init; } = new();
        private readonly ChatComponent[]? InternalChildren = null;
        public ChatComponent[]? Children
        {
            get => [.. InternalChildren];
            init => InternalChildren = [.. value];
        }
        internal ChatComponent()
        {

        }
        internal abstract void TypeNbtSerialize(Stream stream);
        internal abstract void TypeJsonSerialize(StringBuilder json);
        internal void NbtSerialize(Stream stream)
        {
            stream.WriteU8(0x0A);
            InternalNbtSerialize(stream);
        }
        private void InternalNbtSerialize(Stream stream)
        {
            Styling?.NbtSerialize(stream);
            TypeNbtSerialize(stream);
            if (Children is not null)
            {
                stream.WriteU8(9);
                stream.WriteString("extra", SizePrefix.U16);
                stream.WriteU8(10);
                stream.WriteS32(Children.Length);
                for (int i = 0; i < Children.Length; i++) Children[i].InternalNbtSerialize(stream);
            }
            stream.WriteU8(0);
        }
        internal string JsonSerialize() //TODO: get rid of this mess
        {
            StringBuilder json = new();
            json.Append('{');
            if (Styling?.Color.HasValue ?? false)
            {
                json.Append("\"color\":\"");
                json.Append('#');
                json.Append(Styling.Color.Value.R.ToString("X2"));
                json.Append(Styling.Color.Value.G.ToString("X2"));
                json.Append(Styling.Color.Value.B.ToString("X2"));
                json.Append("\",");
            }
            if (Styling?.Bold.HasValue ?? false)
            {
                json.Append("\"bold\":");
                json.Append(Styling.Bold.Value ? "true" : "false");
                json.Append(',');
            }
            if (Styling?.Italic.HasValue ?? false)
            {
                json.Append("\"italic\":");
                json.Append(Styling.Italic.Value ? "true" : "false");
                json.Append(',');
            }
            if (Styling?.Underlined.HasValue ?? false)
            {
                json.Append("\"underlined\":");
                json.Append(Styling.Underlined.Value ? "true" : "false");
                json.Append(',');
            }
            if (Styling?.Strikethrough.HasValue ?? false)
            {
                json.Append("\"strikethrough\":");
                json.Append(Styling.Strikethrough.Value ? "true" : "false");
                json.Append(',');
            }
            if (Styling?.Obfuscated.HasValue ?? false)
            {
                json.Append("\"obfuscated\":");
                json.Append(Styling.Obfuscated.Value ? "true" : "false");
                json.Append(',');
            }
            if (Styling?.Font is not null)
            {
                json.Append("\"font\":\"");
                json.Append(HttpUtility.JavaScriptStringEncode(Styling.Font, false));
                json.Append("\",");
            }
            if (Styling?.Insertion is not null)
            {
                json.Append("\"insertion\":\"");
                json.Append(HttpUtility.JavaScriptStringEncode(Styling.Insertion, false));
                json.Append("\",");
            }
            if (Styling?.HoverText is not null)
            {
                json.Append("\"hoverEvent\":{\"action\":\"show_text\",\"value\":\"");
                json.Append(HttpUtility.JavaScriptStringEncode(Styling.HoverText, false));
                json.Append("\"},");
            }
            TypeJsonSerialize(json);
            if (Children is not null)
            {
                json.Append("\"extra\":[");
                foreach (ChatComponent component in Children)
                {
                    json.Append(component.JsonSerialize());
                    json.Append(',');
                }
                if (Children.Length > 0) json.Remove(json.Length - 1, 1);
                json.Append("],");
            }
            json.Remove(json.Length - 1, 1);
            json.Append('}');
            return json.ToString();
        }
    }
}
