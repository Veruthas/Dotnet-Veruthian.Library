using System.Collections.Generic;
using Soedeum.Dotnet.Library.Collections;

namespace Soedeum.Dotnet.Library.Text
{
    public static class TextUtility
    {
        public static IEnumerator<CodePoint> AsCodePoints(this IEnumerator<char> chars)
        {
            var decoder = new Utf16.CharDecoder();

            return DecodeValues(chars, decoder, "Missing trailing surrogate.");
        }

        public static IEnumerator<CodePoint> AsUtf8CodePoints(this IEnumerator<byte> bytes)
        {
            var decoder = new Utf8.ByteDecoder();

            return DecodeValues(bytes, decoder, "Ill-formed Utf8.");
        }

        public static IEnumerator<CodePoint> AsUtf16CodePoints(this IEnumerator<byte> bytes, bool littleEndian = false)
        {
            var decoder = new Utf16.ByteDecoder(littleEndian);

            return DecodeValues(bytes, decoder, "Ill-formed Utf16.");
        }

        public static IEnumerator<CodePoint> AsUtf32CodePoints(this IEnumerator<byte> bytes, bool littleEndian = false)
        {
            var decoder = new Utf32.ByteDecoder(littleEndian);

            return DecodeValues(bytes, decoder, "Ill-formed Utf32.");
        }

        public static IEnumerator<CodePoint> DecodeValues<T, TDecoder>(IEnumerator<T> items, TDecoder decoder, string onIncomplete)
            where TDecoder : ITransformer<T, CodePoint>
        {
            bool result = true;

            while (items.MoveNext())
            {
                result = decoder.TryProcess(items.Current, out var codepoint);

                if (result)
                    yield return codepoint;
            }

            if (!result)
                throw new InvalidCodePointException(onIncomplete);
        }


        public static EnumerableAdapter<CodePoint> AsCodePoints(this IEnumerable<char> chars)
        {
            return new EnumerableAdapter<CodePoint>(AsCodePoints(chars.GetEnumerator()));
        }

        public static EnumerableAdapter<CodePoint> AsUtf8CodePoints(this IEnumerable<byte> bytes)
        {
            return new EnumerableAdapter<CodePoint>(AsUtf8CodePoints(bytes.GetEnumerator()));
        }

        public static EnumerableAdapter<CodePoint> AsUtf16CodePoints(this IEnumerable<byte> bytes, bool littleEndian = false)
        {
            return new EnumerableAdapter<CodePoint>(AsUtf16CodePoints(bytes.GetEnumerator(), littleEndian));
        }

        public static EnumerableAdapter<CodePoint> AsUtf32CodePoints(this IEnumerable<byte> bytes, bool littleEndian = false)
        {
            return new EnumerableAdapter<CodePoint>(AsUtf32CodePoints(bytes.GetEnumerator(), littleEndian));
        }
    }
}