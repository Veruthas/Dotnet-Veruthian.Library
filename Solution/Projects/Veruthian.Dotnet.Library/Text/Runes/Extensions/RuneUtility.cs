using System;
using System.Collections.Generic;
using Veruthian.Dotnet.Library.Numeric;
using Veruthian.Dotnet.Library.Text.Runes.Encodings;

namespace Veruthian.Dotnet.Library.Text.Runes.Extensions
{
    public static class RuneUtility
    {
        // Decode to CodePoint
        public static IEnumerable<Rune> DecodeValues<T, TDecoder>(IEnumerable<T> items, TDecoder decoder, string onIncomplete)
            where TDecoder : ITransformer<T, uint?>
        {
            var enumerator = items.GetEnumerator();

            if (items == null)
                throw new ArgumentNullException("items");

            uint? result = 0;

            while (enumerator.MoveNext())
            {
                result = decoder.Process(enumerator.Current);

                if (result != null)
                    yield return (Rune)result.GetValueOrDefault();
            }

            if (result == null)
                throw new RuneException(onIncomplete);
        }

        // Utf8 -> CodePoint
        public static IEnumerable<Rune> AsUtf8CodePoints(this IEnumerable<byte> bytes)
        {
            var decoder = new Utf8.ByteDecoder();

            return DecodeValues(bytes, decoder, "Ill-formed Utf8.");
        }

        // Utf16 -> CodePoint
        public static IEnumerable<Rune> AsUtf16CodePoints(this IEnumerable<byte> bytes, ByteOrder endianness = ByteOrder.LittleEndian)
        {
            var decoder = new Utf16.ByteDecoder(endianness);

            return DecodeValues(bytes, decoder, "Ill-formed Utf16.");
        }


        // Utf32 -> CodePoint
        public static IEnumerable<Rune> AsUtf32CodePoints(this IEnumerable<byte> bytes, ByteOrder endianness = ByteOrder.LittleEndian)
        {
            var decoder = new Utf32.ByteDecoder(endianness);

            return DecodeValues(bytes, decoder, "Ill-formed Utf32.");
        }


        // Char -> CodePoint
        public static IEnumerable<Rune> ToCodePoints(this IEnumerable<char> chars)
        {
            var decoder = new Utf16.CharDecoder();

            return DecodeValues(chars, decoder, Utf16.MissingTrailingSurrogateMessage());
        }

        // String -> CodePoint
        public static IEnumerable<Rune> ToCodePoints(this string value)
        {
            if (value == null)
                throw new ArgumentNullException("value");

            return UncheckedToCodePoints(value, 0, value.Length);
        }

        public static IEnumerable<Rune> ToCodePoints(this string value, int start)
        {
            if (value == null)
                throw new ArgumentNullException("value");

            return UncheckedToCodePoints(value, start, value.Length - start);
        }

        public static IEnumerable<Rune> ToCodePoints(this string value, int start, int amount)
        {
            if (value == null)
                throw new ArgumentNullException("value");


            return UncheckedToCodePoints(value, start, amount);
        }

        private static IEnumerable<Rune> UncheckedToCodePoints(string value, int start, int amount)
        {
            if (start < 0 || start > value.Length)
                throw new ArgumentOutOfRangeException("start");

            if (amount < 0 || start + amount > value.Length)
                throw new ArgumentOutOfRangeException("amount");


            var codepoints = new Rune[amount];


            var decoder = new Utf16.CharDecoder();

            bool result = true;

            for (int i = start; i < amount; i++)
            {
                var utf32 = decoder.Process(value[i]);

                if (utf32 != null)
                    yield return (Rune)utf32.GetValueOrDefault();
            }


            if (!result)
                throw new RuneException(Utf16.MissingTrailingSurrogateMessage());

        }


        // String -> CodePoint[]
        public static Rune[] ToCodePointArray(this string value)
        {
            if (value == null)
                throw new ArgumentNullException("value");

            return UncheckedToCodePointArray(value, 0, value.Length);
        }

        public static Rune[] ToCodePointArray(this string value, int start)
        {
            if (value == null)
                throw new ArgumentNullException("value");

            return UncheckedToCodePointArray(value, start, value.Length - start);
        }

        public static Rune[] ToCodePointArray(this string value, int start, int amount)
        {
            if (value == null)
                throw new ArgumentNullException(value);

            return UncheckedToCodePointArray(value, start, amount);
        }

        private static Rune[] UncheckedToCodePointArray(string value, int start, int amount)
        {

            if (start < 0 || start > value.Length)
                throw new ArgumentOutOfRangeException("start");

            if (amount < 0 || start + amount > value.Length)
                throw new ArgumentOutOfRangeException("amount");


            var codepoints = new Rune[amount];

            int index = 0;


            var decoder = new Utf16.CharDecoder();

            bool result = true;

            for (int i = start; i < amount; i++)
            {
                var utf32 = decoder.Process(value[i]);

                if (utf32 != null)
                    codepoints[index++] = (Rune)utf32.GetValueOrDefault();
            }


            if (!result)
                throw new RuneException(Utf16.MissingTrailingSurrogateMessage());


            if (index < codepoints.Length)
                Array.Resize(ref codepoints, index);

            return codepoints;
        }

        public static bool IsNullOrEmpty(this RuneString value) => RuneString.IsNullOrEmpty(value);



        // CodeString
        public static RuneString ToCodeString(this ICollection<Rune> codepoints)
        {
            return new RuneString(codepoints);
        }
        public static RuneString ToCodeString(this IList<Rune> codepoints, int index)
        {
            return new RuneString(codepoints, index);
        }
        public static RuneString ToCodeString(this IList<Rune> codepoints, int index, int length)
        {
            return new RuneString(codepoints, index, length);
        }

        public static RuneString ToCodeString(this IEnumerable<Rune> codepoints)
        {
            return new RuneString(codepoints);
        }

        public static RuneString ToCodeString(this IEnumerable<char> chars)
        {
            return new RuneString(ToCodePoints(chars));
        }

        public static RuneString ToCodeString(this string value)
        {
            return new RuneString(value);
        }

        public static RuneString ToCodeString(this string value, int start)
        {
            return new RuneString(value, start);
        }

        public static RuneString ToCodeString(this string value, int start, int amount)
        {
            return new RuneString(value, start, amount);
        }
    }
}