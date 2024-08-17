using Net.Myzuc.ShioLib;
using System.IO;
using System.Text;
using System.Web;
using System.Drawing;

namespace Net.Myzuc.PurpleStainedGlass.Protocol.Objects.Chat
{
    public sealed class ChatKeybind : ChatComponent
    {
        public string Keybind { get; }
        public ChatKeybind(string keybind)
        {
            Keybind = keybind;
        }
        internal override void TypeNbtSerialize(Stream stream)
        {
            stream.WriteU8(8);
            stream.WriteString("type", SizePrefix.U16);
            stream.WriteString("keybind", SizePrefix.U16);
            stream.WriteU8(8);
            stream.WriteString("keybind", SizePrefix.U16);
            stream.WriteString(Keybind, SizePrefix.U16);
        }
        internal override void TypeJsonSerialize(StringBuilder json)
        {
            json.Append("\"keybind\":\"");
            json.Append(HttpUtility.JavaScriptStringEncode(Keybind, false));
            json.Append("\",");
        }
    }
}
