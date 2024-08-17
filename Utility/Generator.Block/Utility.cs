using System.Text;

namespace Net.Myzuc.PurpleStainedGlass.Generator
{
    public static class Utility
    {
        public static string ToCamelCase(string text, bool firstUppercase)
        {
            StringBuilder builder = new();
            for (int i = 0; i < text.Length; i++)
            {
                char c = text[i];
                if (c == '_') continue;
                builder.Append(((firstUppercase && i == 0) || ((firstUppercase || i != 0) && text[i - 1] == '_')) ? char.ToUpper(c) : c);
            }
            return builder.ToString();
        }
    }
}
