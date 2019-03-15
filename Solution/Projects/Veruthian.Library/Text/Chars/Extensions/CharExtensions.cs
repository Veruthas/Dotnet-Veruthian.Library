using System.Text;

namespace Veruthian.Library.Text.Chars.Extensions
{
    public static class CharExtensions
    {
        // Printable Chars
        public static string GetAsPrintable(this char value)
        {
            switch (value)
            {
                case '\n':
                    return "\\n";
                case '\r':
                    return "\\r";
                case '\t':
                    return "\\t";
                case '\0':
                    return "\\0";
                default:
                    return value.ToString();
            }
        }

        public static string GetAsPrintable(this string value)
        {
            var builder = new StringBuilder();

            foreach (char c in value)
                builder.Append(GetAsPrintable(c));

            return builder.ToString();
        }

    
        // String extensions
        public static bool IsNullOrEmpty(this string value) => string.IsNullOrEmpty(value);

        public static bool IsNullOrWhitespace(this string value) => string.IsNullOrWhiteSpace(value);
    }
}