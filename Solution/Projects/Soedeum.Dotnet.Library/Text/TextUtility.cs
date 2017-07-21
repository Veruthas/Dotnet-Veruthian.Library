using System;
using System.Collections.Generic;
using Soedeum.Dotnet.Library.Data;
using Soedeum.Dotnet.Library.Numeric;
using Soedeum.Dotnet.Library.Text.Encodings;

namespace Soedeum.Dotnet.Library.Text
{
    public static class TextUtility
    {
        // Decode to CodePoint
        public static IEnumerator<CodePoint> DecodeValues<T, TDecoder>(IEnumerator<T> items, TDecoder decoder, string onIncomplete)
            where TDecoder : ITransformer<T, uint?>
        {
            if (items == null)
                throw new ArgumentNullException("items");

            uint? result = 0;

            while (items.MoveNext())
            {
                result = decoder.Process(items.Current);

                if (result != null)
                    yield return (CodePoint)result.GetValueOrDefault();
            }

            if (result == null)
                throw new CodePointException(onIncomplete);
        }

        // Utf8 -> CodePoint
        public static IEnumerator<CodePoint> AsUtf8CodePoints(this IEnumerable<byte> bytes)
        {
            return AsUtf8CodePoints(bytes.GetEnumerator());
        }

        public static IEnumerator<CodePoint> AsUtf8CodePoints(this IEnumerator<byte> bytes)
        {
            var decoder = new Utf8.ByteDecoder();

            return DecodeValues(bytes, decoder, "Ill-formed Utf8.");
        }

        // Utf16 -> CodePoint
        public static IEnumerator<CodePoint> AsUtf16CodePoints(this IEnumerable<byte> bytes, ByteOrder endianness = ByteOrder.LittleEndian)
        {
            return AsUtf16CodePoints(bytes.GetEnumerator(), endianness);
        }
        public static IEnumerator<CodePoint> AsUtf16CodePoints(this IEnumerator<byte> bytes, ByteOrder endianness = ByteOrder.LittleEndian)
        {
            var decoder = new Utf16.ByteDecoder(endianness);

            return DecodeValues(bytes, decoder, "Ill-formed Utf16.");
        }


        // Utf32 -> CodePoint
        public static IEnumerator<CodePoint> AsUtf32CodePoints(this IEnumerable<byte> bytes, ByteOrder endianness = ByteOrder.LittleEndian)
        {
            return AsUtf32CodePoints(bytes.GetEnumerator(), endianness);
        }

        public static IEnumerator<CodePoint> AsUtf32CodePoints(this IEnumerator<byte> bytes, ByteOrder endianness = ByteOrder.LittleEndian)
        {
            var decoder = new Utf32.ByteDecoder(endianness);

            return DecodeValues(bytes, decoder, "Ill-formed Utf32.");
        }


        // Char -> CodePoint
        public static IEnumerator<CodePoint> ToCodePoints(this IEnumerator<char> chars)
        {
            var decoder = new Utf16.CharDecoder();

            return DecodeValues(chars, decoder, Utf16.MissingTrailingSurrogateMessage());
        }

        public static IEnumerator<CodePoint> ToCodePoints(this IEnumerable<char> chars)
        {
            return ToCodePoints(chars.GetEnumerator());
        }

        // String -> CodePoint
        public static IEnumerator<CodePoint> ToCodePoints(this string value)
        {
            if (value == null)
                throw new ArgumentNullException("value");

            return UncheckedToCodePoints(value, 0, value.Length);
        }

        public static IEnumerator<CodePoint> ToCodePoints(this string value, int start)
        {
            if (value == null)
                throw new ArgumentNullException("value");

            return UncheckedToCodePoints(value, start, value.Length - start);
        }

        public static IEnumerator<CodePoint> ToCodePoints(this string value, int start, int amount)
        {
            if (value == null)
                throw new ArgumentNullException("value");


            return UncheckedToCodePoints(value, start, amount);
        }

        private static IEnumerator<CodePoint> UncheckedToCodePoints(string value, int start, int amount)
        {
            if (start < 0 || start > value.Length)
                throw new ArgumentOutOfRangeException("start");

            if (amount < 0 || start + amount > value.Length)
                throw new ArgumentOutOfRangeException("amount");


            var codepoints = new CodePoint[amount];


            var decoder = new Utf16.CharDecoder();

            bool result = true;

            for (int i = start; i < amount; i++)
            {
                var utf32 = decoder.Process(value[i]);

                if (utf32 != null)
                    yield return (CodePoint)utf32.GetValueOrDefault();
            }


            if (!result)
                throw new CodePointException(Utf16.MissingTrailingSurrogateMessage());

        }


        // String -> CodePoint[]
        public static CodePoint[] ToCodePointArray(this string value)
        {
            if (value == null)
                throw new ArgumentNullException("value");

            return UncheckedToCodePointArray(value, 0, value.Length);
        }

        public static CodePoint[] ToCodePointArray(this string value, int start)
        {
            if (value == null)
                throw new ArgumentNullException("value");

            return UncheckedToCodePointArray(value, start, value.Length - start);
        }

        public static CodePoint[] ToCodePointArray(this string value, int start, int amount)
        {
            if (value == null)
                throw new ArgumentNullException(value);

            return UncheckedToCodePointArray(value, start, amount);
        }

        private static CodePoint[] UncheckedToCodePointArray(string value, int start, int amount)
        {

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
                var utf32 = decoder.Process(value[i]);

                if (utf32 != null)
                    codepoints[index++] = (CodePoint)utf32.GetValueOrDefault();
            }


            if (!result)
                throw new CodePointException(Utf16.MissingTrailingSurrogateMessage());


            if (index < codepoints.Length)
                Array.Resize(ref codepoints, index);

            return codepoints;
        }


        public static bool IsNullOrEmpty(this string value) => string.IsNullOrEmpty(value);

        public static bool IsNullOrWhitespace(this string value) => string.IsNullOrWhiteSpace(value);


        public static bool IsNullOrEmpty(this CodeString value) => CodeString.IsNullOrEmpty(value);



        // CodeString
        public static CodeString ToCodeString(this ICollection<CodePoint> codepoints)
        {
            return new CodeString(codepoints);
        }
        public static CodeString ToCodeString(this IList<CodePoint> codepoints, int index)
        {
            return new CodeString(codepoints, index);
        }
        public static CodeString ToCodeString(this IList<CodePoint> codepoints, int index, int length)
        {
            return new CodeString(codepoints, index, length);
        }

        public static CodeString ToCodeString(this IEnumerable<CodePoint> codepoints)
        {
            return new CodeString(codepoints);
        }

        public static CodeString ToCodeString(this IEnumerable<char> chars)
        {
            return new CodeString(ToCodePoints(chars));
        }

        public static CodeString ToCodeString(this string value)
        {
            return new CodeString(value);
        }

        public static CodeString ToCodeString(this string value, int start)
        {
            return new CodeString(value, start);
        }

        public static CodeString ToCodeString(this string value, int start, int amount)
        {
            return new CodeString(value, start, amount);
        }
    }
}