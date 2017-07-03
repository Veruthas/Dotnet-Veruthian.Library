using System;
using System.Collections.Generic;
using Soedeum.Dotnet.Library.Collections;
using Soedeum.Dotnet.Library.Text.Encodings;

namespace Soedeum.Dotnet.Library.Text
{
    public static class TextUtility
    {
        // Decode to CodePoint
        public static IEnumerator<CodePoint> DecodeValues<T, TDecoder>(IEnumerator<T> items, TDecoder decoder, string onIncomplete)
            where TDecoder : ITransformer<T, uint>
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

        // Utf8 -> CodePoint
        public static EnumerableAdapter<CodePoint> AsUtf8CodePoints(this IEnumerable<byte> bytes)
        {
            return new EnumerableAdapter<CodePoint>(AsUtf8CodePoints(bytes.GetEnumerator()));
        }

        public static IEnumerator<CodePoint> AsUtf8CodePoints(this IEnumerator<byte> bytes)
        {
            var decoder = new Utf8.ByteDecoder();

            return DecodeValues(bytes, decoder, "Ill-formed Utf8.");
        }

        // Utf16 -> CodePoint
        public static EnumerableAdapter<CodePoint> AsUtf16CodePoints(this IEnumerable<byte> bytes, bool littleEndian = false)
        {
            return new EnumerableAdapter<CodePoint>(AsUtf16CodePoints(bytes.GetEnumerator(), littleEndian));
        }
        public static IEnumerator<CodePoint> AsUtf16CodePoints(this IEnumerator<byte> bytes, bool littleEndian = false)
        {
            var decoder = new Utf16.ByteDecoder(littleEndian);

            return DecodeValues(bytes, decoder, "Ill-formed Utf16.");
        }


        // Utf32 -> CodePoint
        public static EnumerableAdapter<CodePoint> AsUtf32CodePoints(this IEnumerable<byte> bytes, bool littleEndian = false)
        {
            return new EnumerableAdapter<CodePoint>(AsUtf32CodePoints(bytes.GetEnumerator(), littleEndian));
        }

        public static IEnumerator<CodePoint> AsUtf32CodePoints(this IEnumerator<byte> bytes, bool littleEndian = false)
        {
            var decoder = new Utf32.ByteDecoder(littleEndian);

            return DecodeValues(bytes, decoder, "Ill-formed Utf32.");
        }


        // Char -> CodePoint
        public static IEnumerator<CodePoint> ToCodePoints(this IEnumerator<char> chars)
        {
            var decoder = new Utf16.CharDecoder();

            return DecodeValues(chars, decoder, "Missing trailing surrogate.");
        }

        public static EnumerableAdapter<CodePoint> ToCodePoints(this IEnumerable<char> chars)
        {
            return new EnumerableAdapter<CodePoint>(ToCodePoints(chars.GetEnumerator()));
        }


        // String -> CodePoint[]
        public static CodePoint[] ToCodePoints(this string value)
        {
            if (value == null)
                return null;
            else
                return ToCodePoints(value, 0, value.Length);
        }

        public static CodePoint[] ToCodePoints(this string value, int start)
        {
            if (value == null)
                return null;
            else
                return ToCodePoints(value, start, value.Length - start);
        }

        public static CodePoint[] ToCodePoints(this string value, int start, int amount)
        {
            if (value == null)
                return null;

            if (start < 0 || start > value.Length)
                throw new ArgumentOutOfRangeException("start");

            if (amount < 0 || start + amount > value.Length)
                throw new ArgumentOutOfRangeException("amount");


            var codepoints = new CodePoint[amount];

            int index = 0;


            var decoder = new Utf16.CharDecoder();

            bool result = true;

            for (int i = start; i < amount; i++)
            {
                result = decoder.TryProcess(value[i], out var codepoint);

                if (result)
                    codepoints[index++] = codepoint;
            }


            // Hack? If missing trailing surrogate, error
            if (!result)
                decoder.TryProcess('\0', out var codepoint);


            if (index < codepoints.Length)
                Array.Resize(ref codepoints, index);

            return codepoints;
        }


        public static bool IsNullOrEmpty(this string value) => string.IsNullOrEmpty(value);

        public static bool IsNullOrWhitespace(this string value) => string.IsNullOrWhiteSpace(value);


        public static bool IsNullOrEmpty(this CodeString value) => CodeString.IsNullOrEmpty(value);
    }
}