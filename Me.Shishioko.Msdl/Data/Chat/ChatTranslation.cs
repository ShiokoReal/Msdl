using Net.Myzuc.ShioLib;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Drawing;

namespace Me.Shishioko.Msdl.Data.Chat
{
    public sealed class ChatTranslation : ChatComponent
    {
        public string Key;
        public IEnumerable<string>? Arguments;
        public ChatTranslation(string translate, IEnumerable<string>? with = null)
        {
            Key = translate;
            Arguments = with;
        }
        internal override void TypeSerialize(Stream stream)
        {
            stream.WriteU8(8);
            stream.WriteString("type", SizePrefix.U16);
            stream.WriteString("translatable", SizePrefix.U16);
            stream.WriteU8(8);
            stream.WriteString("translate", SizePrefix.U16);
            stream.WriteString(Key, SizePrefix.U16);
            if (Arguments is not null)
            {
                stream.WriteU8(9);
                stream.WriteString("with", SizePrefix.U16);
                stream.WriteU8(8);
                stream.WriteS32(Arguments.Count());
                foreach (string argument in Arguments)
                {
                    stream.WriteString(argument, SizePrefix.U16);
                }
            }
        }
        internal override void TypeTextSerialize(StringBuilder json)
        {
            json.Append("\"type\":\"translatable\",\"translate\":\"");
            json.Append(HttpUtility.JavaScriptStringEncode(Key, false));
            if (Arguments is not null)
            {
                json.Append("\",\"with\":[");
                foreach (string argument in Arguments)
                {
                    json.Append('\"');
                    json.Append(HttpUtility.JavaScriptStringEncode(argument, false));
                    json.Append("\",");
                }
                if (Arguments.Any()) json.Remove(json.Length - 1, 1);
                json.Append(']');
            }
            json.Append("\",");
        }
        public override string ToString()
        {
            return $"[{Key}({string.Join(", ", Arguments ?? [])})]" + base.ToString();
        }
        internal override void ToANSI(StringBuilder builder, bool bold, bool italic, bool underlined, bool strikethrough, bool obfuscated, Color color)
        {
            builder.Append("\u001b[0m");
            if (bold) builder.Append("\u001b[1m");
            if (italic) builder.Append("\u001b[3m");
            if (underlined) builder.Append("\u001b[4m");
            if (strikethrough) builder.Append("\u001b[9m");
            if (obfuscated) builder.Append("\u001b[5m");
            builder.Append("\u001b[7m");
            builder.Append($"\u001b[38;2;{color.R};{color.G};{color.B}m");
            builder.Append($"[{Key}({string.Join(", ", Arguments ?? [])})]");
            base.ToANSI(builder, bold, italic, underlined, strikethrough, obfuscated, color);
        }
    }
}
