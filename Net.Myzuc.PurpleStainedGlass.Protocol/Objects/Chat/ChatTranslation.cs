using Net.Myzuc.PurpleStainedGlass.Protocol.Objects;
using Net.Myzuc.ShioLib;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace Net.Myzuc.PurpleStainedGlass.Protocol.Objects.Chat
{
    public sealed class ChatTranslation : ChatComponent
    {
        public Translation Translation { get; }
        public ChatTranslation(Translation translation)
        {
            Translation = translation;
        }
        internal override void TypeNbtSerialize(Stream stream)
        {
            stream.WriteU8(8);
            stream.WriteString("type", SizePrefix.U16);
            stream.WriteString("translatable", SizePrefix.U16);
            stream.WriteU8(8);
            stream.WriteString("translate", SizePrefix.U16);
            stream.WriteString(Translation.Key, SizePrefix.U16);
            if (Translation.Arguments is not null)
            {
                stream.WriteU8(9);
                stream.WriteString("with", SizePrefix.U16);
                stream.WriteU8(8);
                stream.WriteS32(Translation.Arguments.Length);
                for (int i = 0; i < Translation.Arguments.Length; i++) stream.WriteString(Translation.Arguments[i], SizePrefix.U16);
            }
        }
        internal override void TypeJsonSerialize(StringBuilder json)
        {
            json.Append("\"translate\":\"");
            json.Append(HttpUtility.JavaScriptStringEncode(Translation.Key, false));
            if (Translation.Arguments is not null)
            {
                json.Append("\",\"with\":[");
                foreach (string argument in Translation.Arguments)
                {
                    json.Append('\"');
                    json.Append(HttpUtility.JavaScriptStringEncode(argument, false));
                    json.Append("\",");
                }
                if (Translation.Arguments.Any()) json.Remove(json.Length - 1, 1);
                json.Append(']');
            }
            json.Append("\",");
        }
    }
}
