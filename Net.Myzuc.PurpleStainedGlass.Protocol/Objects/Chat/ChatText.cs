using Net.Myzuc.ShioLib;
using System.Drawing;
using System.IO;
using System.Text;
using System.Web;

namespace Net.Myzuc.PurpleStainedGlass.Protocol.Objects.Chat
{
    public sealed class ChatText : ChatComponent
    {
        public string Text { get; }
        public ChatText(string text)
        {
            Text = text;
        }
        internal override void TypeNbtSerialize(Stream stream)
        {
            stream.WriteU8(8);
            stream.WriteString("type", SizePrefix.U16);
            stream.WriteString("text", SizePrefix.U16);
            stream.WriteU8(8);
            stream.WriteString("text", SizePrefix.U16);
            stream.WriteString(Text, SizePrefix.U16);
        }
        internal override void TypeJsonSerialize(StringBuilder json)
        {
            json.Append("\"text\":\"");
            json.Append(HttpUtility.JavaScriptStringEncode(Text, false));
            json.Append("\",");
        }
    }
}
