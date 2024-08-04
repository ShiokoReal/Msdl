using System.Drawing;
using System.Collections.Generic;
using System.IO;
using Net.Myzuc.ShioLib;
using System.Linq;
using System.Text;
using System.Web;

namespace Me.Shishioko.Msdl.Data.Chat
{
    public abstract class ChatComponent
    {
        public bool? Bold;
        public bool? Italic;
        public bool? Underlined;
        public bool? Strikethrough;
        public bool? Obfuscated;
        public string? Font;
        public Color? Color;
        public string? Insertion;
        public ClickEvent? ClickEvent;
        public string? HoverText;
        public IEnumerable<ChatComponent>? Extra;
        internal ChatComponent()
        {
            Bold = null;
            Italic = null;
            Underlined = null;
            Strikethrough = null;
            Obfuscated = null;
            Font = null;
            Color = null;
            Insertion = null;
            Extra = null;
        }
        internal abstract void TypeSerialize(Stream stream);
        internal abstract void TypeTextSerialize(StringBuilder json);
        internal void Serialize(Stream stream)
        {
            if (Color.HasValue)
            {
                stream.WriteU8(8);
                stream.WriteString("color", SizePrefix.U16);
                stream.WriteString($"#{Color.Value.R:X2}{Color.Value.G:X2}{Color.Value.B:X2}", SizePrefix.U16, 7);
            }
            if (Bold.HasValue)
            {
                stream.WriteU8(1);
                stream.WriteString("bold", SizePrefix.U16);
                stream.WriteBool(Bold.Value);
            }
            if (Italic.HasValue)
            {
                stream.WriteU8(1);
                stream.WriteString("italic", SizePrefix.U16);
                stream.WriteBool(Italic.Value);
            }
            if (Underlined.HasValue)
            {
                stream.WriteU8(1);
                stream.WriteString("underlined", SizePrefix.U16);
                stream.WriteBool(Underlined.Value);
            }
            if (Strikethrough.HasValue)
            {
                stream.WriteU8(1);
                stream.WriteString("strikethrough", SizePrefix.U16);
                stream.WriteBool(Strikethrough.Value);
            }
            if (Obfuscated.HasValue)
            {
                stream.WriteU8(1);
                stream.WriteString("obfuscated", SizePrefix.U16);
                stream.WriteBool(Obfuscated.Value);
            }
            if (Font is not null)
            {
                stream.WriteU8(8);
                stream.WriteString("font", SizePrefix.U16);
                stream.WriteString(Font, SizePrefix.U16);
            }
            if (Insertion is not null)
            {
                stream.WriteU8(8);
                stream.WriteString("insertion", SizePrefix.U16);
                stream.WriteString(Insertion, SizePrefix.U16);
            }
            if (ClickEvent is not null)
            {
                stream.WriteU8(10);
                stream.WriteString("clickEvent", SizePrefix.U16);
                stream.WriteU8(8);
                stream.WriteString("action", SizePrefix.U16);
                stream.WriteString(ClickEvent.Type, SizePrefix.U16);
                stream.WriteU8(8);
                stream.WriteString("value", SizePrefix.U16);
                stream.WriteString(ClickEvent.Content, SizePrefix.U16);
                stream.WriteU8(0);
            }
            if (HoverText is not null)
            {
                stream.WriteU8(10);
                stream.WriteString("hoverEvent", SizePrefix.U16);
                stream.WriteU8(8);
                stream.WriteString("action", SizePrefix.U16);
                stream.WriteString("show_text", SizePrefix.U16);
                stream.WriteU8(8);
                stream.WriteString("value", SizePrefix.U16);
                stream.WriteString(HoverText, SizePrefix.U16);
                stream.WriteU8(0);
            }
            TypeSerialize(stream);
            if (Extra is not null)
            {
                stream.WriteU8(9);
                stream.WriteString("extra", SizePrefix.U16);
                stream.WriteU8(10);
                stream.WriteS32(Extra.Count());
                foreach(ChatComponent component in Extra)
                {
                    component.Serialize(stream);
                }
            }
            stream.WriteU8(0);
        }
        internal string TextSerialize()
        {
            StringBuilder json = new();
            json.Append('{');
            if (Color.HasValue)
            {
                json.Append("\"color\":\"");
                json.Append('#');
                json.Append(Color.Value.R.ToString("X2"));
                json.Append(Color.Value.G.ToString("X2"));
                json.Append(Color.Value.B.ToString("X2"));
                json.Append("\",");
            }
            if (Bold.HasValue)
            {
                json.Append("\"bold\":");
                json.Append(Bold.Value ? "true" : "false");
                json.Append(',');
            }
            if (Italic.HasValue)
            {
                json.Append("\"italic\":");
                json.Append(Italic.Value ? "true" : "false");
                json.Append(',');
            }
            if (Underlined.HasValue)
            {
                json.Append("\"underlined\":");
                json.Append(Underlined.Value ? "true" : "false");
                json.Append(',');
            }
            if (Strikethrough.HasValue)
            {
                json.Append("\"strikethrough\":");
                json.Append(Strikethrough.Value ? "true" : "false");
                json.Append(',');
            }
            if (Obfuscated.HasValue)
            {
                json.Append("\"obfuscated\":");
                json.Append(Obfuscated.Value ? "true" : "false");
                json.Append(',');
            }
            if (Font is not null)
            {
                json.Append("\"font\":\"");
                json.Append(HttpUtility.JavaScriptStringEncode(Font, false));
                json.Append("\",");
            }
            if (Insertion is not null)
            {
                json.Append("\"insertion\":\"");
                json.Append(HttpUtility.JavaScriptStringEncode(Insertion, false));
                json.Append("\",");
            }
            if (HoverText is not null)
            {
                json.Append("\"hoverEvent\":{\"action\":\"show_text\",\"value\":\"");
                json.Append(HttpUtility.JavaScriptStringEncode(HoverText, false));
                json.Append("\"},");
            }
            TypeTextSerialize(json);
            if (Extra is not null)
            {
                json.Append("\"extra\":[");
                foreach(ChatComponent component in Extra)
                {
                    json.Append(component.TextSerialize());
                    json.Append(',');
                }
                if (Extra.Any()) json.Remove(json.Length - 1, 1);
                json.Append("],");
            }
            json.Remove(json.Length - 1, 1);
            json.Append('}');
            return json.ToString();
        }
        public override string ToString()
        {
            if (Extra is null) return string.Empty;
            return string.Concat(Extra);
        }
        internal virtual void ToANSI(StringBuilder builder, bool bold, bool italic, bool underlined, bool strikethrough, bool obfuscated, Color color)
        {
            if (Extra is null) return;
            foreach(ChatComponent component in Extra)
            {
                component.ToANSI(builder, component.Bold.HasValue ? component.Bold.Value : bold, component.Italic.HasValue ? component.Italic.Value : italic, component.Underlined.HasValue ? component.Underlined.Value : underlined, component.Strikethrough.HasValue ? component.Strikethrough.Value : strikethrough, component.Obfuscated.HasValue ? component.Obfuscated.Value : obfuscated, component.Color.HasValue ? component.Color.Value : color);
            }
        }
        public string ToANSI()
        {
            StringBuilder builder = new();
            ToANSI(builder, Bold.HasValue ? Bold.Value : false, Italic.HasValue ? Italic.Value : false, Underlined.HasValue ? Underlined.Value : false, Strikethrough.HasValue ? Strikethrough.Value : false, Obfuscated.HasValue ? Obfuscated.Value : false, Color.HasValue ? Color.Value : System.Drawing.Color.White);
            return builder.ToString();
        }
    }
}
