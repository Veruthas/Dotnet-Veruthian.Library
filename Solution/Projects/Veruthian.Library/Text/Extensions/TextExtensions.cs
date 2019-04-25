using System.Collections.Generic;

namespace Veruthian.Library.Text.Extensions
{
    public static class TextExtensions
    {
        public static IEnumerable<U> GetPreBuffered<U>(this IEnumerable<U> values, IEditableText<U> buffer)
        {
            foreach (var value in values)
            {
                buffer.Append(value);

                yield return value;
            }
        }

        public static IEnumerable<U> GetPostBuffered<U>(this IEnumerable<U> values, IEditableText<U> buffer)
        {
            foreach (var value in values)
            {
                yield return value;

                buffer.Append(value);
            }
        }
    }
}