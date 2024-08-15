using Net.Myzuc.PurpleStainedGlass.Protocol.Objects.Chat;
using Net.Myzuc.ShioLib;
using System.Drawing;
using System.IO;

namespace Net.Myzuc.PurpleStainedGlass.Protocol.Objects
{
    public sealed class Styling
    {
        public static string[] Fonts => ["minecraft:default", "minecraft:uniform", "minecraft:alt", "minecraft:illageralt"];
        public bool? Bold { get; init; } = null;
        public bool? Italic { get; init; } = null;
        public bool? Underlined { get; init; } = null;
        public bool? Strikethrough { get; init; } = null;
        public bool? Obfuscated { get; init; } = null;
        public string? Font { get; init; } = null;
        public Color? Color { get; init; } = null;
        public string? Insertion { get; init; } = null;
        public ClickEvent? ClickEvent { get; init; } = null;
        public string? HoverText { get; init; } = null;
        public Styling()
        {

        }
        internal void NbtSerialize(Stream stream)
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
        }
    }
}
