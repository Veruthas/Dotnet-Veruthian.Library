using System.Collections.Generic;

namespace Soedeum.Dotnet.Library.Text
{
    public static class TextUtility
    {
        public static IEnumerator<CodePoint> AsCodePoints(this IEnumerator<char> chars)
        {
            var decoder = new Utf16.CharDecoder();

            bool result = true;

            while(chars.MoveNext())
            {
                result = decoder.TryProcess(chars.Current, out var codepoint);

                if (result)
                    yield return codepoint;
            }

            if (!result)
                throw new InvalidCodePointException("Missing trailing surrogate.");
        }
    }
}