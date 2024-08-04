using Net.Myzuc.ShioLib;
using System.Drawing;
using System.IO;
using System.Text;
using System.Web;

namespace Me.Shishioko.Msdl.Data.Chat
{
    public sealed class ChatText : ChatComponent
    {
        public string Text;
        public ChatText(string text)
        {
            Text = text;
        }
        internal override void TypeSerialize(Stream stream)
        {
            stream.WriteU8(8);
            stream.WriteString("type", SizePrefix.U16);
            stream.WriteString("text", SizePrefix.U16);
            stream.WriteU8(8);
            stream.WriteString("text", SizePrefix.U16);
            stream.WriteString(Text, SizePrefix.U16);
        }
        internal override void TypeTextSerialize(StringBuilder json)
        {
            json.Append("\"type\":\"text\",\"text\":\"");
            json.Append(HttpUtility.JavaScriptStringEncode(Text, false));
            json.Append("\",");
        }
        public override string ToString()
        {
            return Text + base.ToString();
        }
        internal override void ToANSI(StringBuilder builder, bool bold, bool italic, bool underlined, bool strikethrough, bool obfuscated, Color color)
        {
            builder.Append("\u001b[0m");
            if (bold) builder.Append("\u001b[1m");
            if (italic) builder.Append("\u001b[3m");
            if (underlined) builder.Append("\u001b[4m");
            if (strikethrough) builder.Append("\u001b[9m");
            if (obfuscated) builder.Append("\u001b[5m");
            builder.Append($"\u001b[38;2;{color.R};{color.G};{color.B}m");
            builder.Append(Text);
            base.ToANSI(builder, bold, italic, underlined, strikethrough, obfuscated, color);
        }
    }
}
