using System;
using System.Collections.Generic;
using Veruthian.Library.Numeric;
using Veruthian.Library.Text.Encodings;

namespace Veruthian.Library.Text.Runes.Extensions
{
    public static class RuneExtensions
    {
        // Rune Decoder
        public static IEnumerable<Rune> DecodeValues<T, TDecoder>(this IEnumerable<T> items, TDecoder decoder, string onIncomplete)
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


        // byte.Utf8... -> Rune
        public static IEnumerable<Rune> AsUtf8Runes(this IEnumerable<byte> bytes)
        {
            var decoder = new Utf8.ByteDecoder();

            return DecodeValues(bytes, decoder, "Ill-formed Utf8.");
        }

        // byte.Utf16... -> Rune
        public static IEnumerable<Rune> AsUtf16Runes(this IEnumerable<byte> bytes, ByteOrder endianness = ByteOrder.LittleEndian)
        {
            var decoder = new Utf16.ByteDecoder(endianness);

            return DecodeValues(bytes, decoder, "Ill-formed Utf16.");
        }


        // byte.Utf32... -> Rune
        public static IEnumerable<Rune> AsUtf32Runes(this IEnumerable<byte> bytes, ByteOrder endianness = ByteOrder.LittleEndian)
        {
            var decoder = new Utf32.ByteDecoder(endianness);

            return DecodeValues(bytes, decoder, "Ill-formed Utf32.");
        }


        // char... -> Rune...
        public static IEnumerable<Rune> ToRunes(this IEnumerable<char> chars)
        {
            var decoder = new Utf16.CharDecoder();

            return DecodeValues(chars, decoder, Utf16.MissingTrailingSurrogateMessage());
        }


        // string -> Rune...
        public static IEnumerable<Rune> ToRunes(this string value)
        {
            if (value == null)
                throw new ArgumentNullException("value");

            return UncheckedToRunes(value, 0, value.Length);
        }

        public static IEnumerable<Rune> ToRunes(this string value, int start)
        {
            if (value == null)
                throw new ArgumentNullException("value");

            return UncheckedToRunes(value, start, value.Length - start);
        }

        public static IEnumerable<Rune> ToRunes(this string value, int start, int amount)
        {
            if (value == null)
                throw new ArgumentNullException("value");


            return UncheckedToRunes(value, start, amount);
        }

        private static IEnumerable<Rune> UncheckedToRunes(string value, int start, int amount)
        {
            if (start < 0 || start > value.Length)
                throw new ArgumentOutOfRangeException("start");

            if (amount < 0 || start + amount > value.Length)
                throw new ArgumentOutOfRangeException("amount");


            var runes = new Rune[amount];


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


        // string -> Rune[]
        public static Rune[] ToRuneArray(this string value)
        {
            if (value == null)
                throw new ArgumentNullException("value");

            return UncheckedToRuneArray(value, 0, value.Length);
        }

        public static Rune[] ToRuneArray(this string value, int start)
        {
            if (value == null)
                throw new ArgumentNullException("value");

            return UncheckedToRuneArray(value, start, value.Length - start);
        }

        public static Rune[] ToRuneArray(this string value, int start, int amount)
        {
            if (value == null)
                throw new ArgumentNullException(value);

            return UncheckedToRuneArray(value, start, amount);
        }

        private static Rune[] UncheckedToRuneArray(string value, int start, int amount)
        {

            if (start < 0 || start > value.Length)
                throw new ArgumentOutOfRangeException("start");

            if (amount < 0 || start + amount > value.Length)
                throw new ArgumentOutOfRangeException("amount");


            var runes = new Rune[amount];

            int index = 0;


            var decoder = new Utf16.CharDecoder();

            bool result = true;

            for (int i = start; i < amount; i++)
            {
                var utf32 = decoder.Process(value[i]);

                if (utf32 != null)
                    runes[index++] = (Rune)utf32.GetValueOrDefault();
            }


            if (!result)
                throw new RuneException(Utf16.MissingTrailingSurrogateMessage());


            if (index < runes.Length)
                Array.Resize(ref runes, index);

            return runes;
        }



        // -> RuneString
        public static RuneString ToRuneString(this ICollection<Rune> runes)
        {
            return new RuneString(runes);
        }

        public static RuneString ToRuneString(this IList<Rune> runes, int index)
        {
            return new RuneString(runes, index);
        }

        public static RuneString ToRuneString(this IList<Rune> runes, int index, int length)
        {
            return new RuneString(runes, index, length);
        }

        public static RuneString ToRuneString(this IEnumerable<Rune> runes)
        {
            return new RuneString(runes);
        }

        public static RuneString ToRuneString(this IEnumerable<char> chars)
        {
            return new RuneString(ToRunes(chars));
        }

        public static RuneString ToRuneString(this string value)
        {
            return new RuneString(value);
        }

        public static RuneString ToRuneString(this string value, int start)
        {
            return new RuneString(value, start);
        }

        public static RuneString ToRuneString(this string value, int start, int amount)
        {
            return new RuneString(value, start, amount);
        }


        // RuneString
        public static bool IsNullOrEmpty(this RuneString value) => RuneString.IsNullOrEmpty(value);


        // LineTracking
        public static IEnumerable<Rune> ProcessLines(this IEnumerable<Rune> runes, out LineIndexTable lines)
        {
            lines = new LineIndexTable();

            return ProcessLines(runes, lines);
        }

        private static IEnumerable<Rune> ProcessLines(this IEnumerable<Rune> runes, LineIndexTable lines)
        {
            Rune current = '\0';

            foreach (var rune in runes)
            {
                lines.MoveToNext(current, rune);

                yield return rune;

                current = rune;
            }

            lines.MoveToNext(current, '\0');
        }
    }
}