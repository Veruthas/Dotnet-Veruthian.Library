using System;
using System.Collections.Generic;
using Veruthian.Library.Numeric.Binary;
using Veruthian.Library.Processing;
using Veruthian.Library.Text.Encodings;
using Veruthian.Library.Text.Lines;

namespace Veruthian.Library.Text.Runes.Extensions
{
    public static class RuneExtensions
    {
        // Rune Decoder
        public static IEnumerable<Rune> DecodeValues<T, TDecoder>(this IEnumerable<T> items, TDecoder decoder, string onIncomplete)
            where TDecoder : IStepwiseTransformer<T, uint>
        {
            var enumerator = items.GetEnumerator();

            if (items == null)
                throw new ArgumentNullException("items");


            var complete = true;

            while (enumerator.MoveNext())
            {
                var result = decoder.Process(enumerator.Current);

                complete = result.Complete;

                if (complete)
                    yield return (Rune)result.Result;
            }

            if (!complete)
                throw new RuneException(onIncomplete);
        }


        // byte.Utf8... -> Rune
        public static IEnumerable<Rune> FromByteUtf8(this IEnumerable<byte> bytes)
        {
            var decoder = new Utf8.ByteDecoder();

            return DecodeValues(bytes, decoder, "Ill-formed Utf8.");
        }

        // byte.Utf16... -> Rune
        public static IEnumerable<Rune> FromByteUtf16(this IEnumerable<byte> bytes, ByteOrder endianness = ByteOrder.LittleEndian)
        {
            var decoder = new Utf16.ByteDecoder(endianness);

            return DecodeValues(bytes, decoder, "Ill-formed Utf16.");
        }


        // byte.Utf32... -> Rune
        public static IEnumerable<Rune> FromByteUtf32(this IEnumerable<byte> bytes, ByteOrder endianness = ByteOrder.LittleEndian)
        {
            var decoder = new Utf32.ByteDecoder(endianness);

            return DecodeValues(bytes, decoder, "Ill-formed Utf32.");
        }


        // Rune... -> byte.Utf8
        public static IEnumerable<byte> ToByteUtf8(this IEnumerable<Rune> runes)
        {
            foreach(var rune in runes)
            {
                var bytes = rune.ToUtf8();

                for (int i = 0; i < bytes.ByteCount; i++)
                    yield return bytes.GetByte(i);
            }
        }

        // Rune... -> byte.Utf16
        public static IEnumerable<byte> ToByteUtf16(this IEnumerable<Rune> runes, ByteOrder endianness = ByteOrder.LittleEndian)
        {
            foreach (var rune in runes)
            {
                var bytes = rune.ToUtf16(endianness);

                for (int i = 0; i < bytes.ByteCount; i++)
                    yield return bytes.GetByte(i);
            }
        }

        // Rune... -> byte.Utf32
        public static IEnumerable<byte> ToByteUtf32(this IEnumerable<Rune> runes, ByteOrder endianness = ByteOrder.LittleEndian)
        {
            foreach (var rune in runes)
            {
                var bytes = rune.ToUtf32(endianness);

                for (int i = 0; i < bytes.ByteCount; i++)
                    yield return bytes.GetByte(i);
            }
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

            bool complete = true;

            for (int i = start; i < amount; i++)
            {
                var result = decoder.Process(value[i]);

                complete = result.Complete;

                if (complete)
                    yield return (Rune)result.Result;
            }


            if (!complete)
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

            bool complete = true;

            for (int i = start; i < amount; i++)
            {
                var result = decoder.Process(value[i]);

                complete = result.Complete;

                if (complete)
                    runes[index++] = (Rune)result.Result;
            }


            if (!complete)
                throw new RuneException(Utf16.MissingTrailingSurrogateMessage());


            if (index < runes.Length)
                Array.Resize(ref runes, index);

            return runes;
        }



        // -> RuneString
        public static RuneString ToRuneString(this IEnumerable<Rune> runes)
        {
            return RuneString.Extract(runes);
        }

        public static RuneString ToRuneString(this IEnumerable<char> chars)
        {
            return RuneString.Extract(ToRunes(chars));
        }

        public static RuneString ToRuneString(this string value)
        {
            return RuneString.From(value);
        }

        public static RuneString ToRuneString(this string value, int start)
        {
            return RuneString.From(value.ToRuneArray(), start);
        }

        public static RuneString ToRuneString(this string value, int start, int amount)
        {
            return RuneString.From(value.ToRuneArray(), start, amount);
        }


        // RuneString
        public static bool IsNullOrEmpty(this RuneString value) => RuneString.IsNullOrEmpty(value);


        // Line Table
        public static IEnumerable<Rune> ProcessLines(this IEnumerable<Rune> runes, out RuneLineTable lines)
        {
            lines = new RuneLineTable();

            return ProcessLines(runes, lines);
        }

        private static IEnumerable<Rune> ProcessLines(this IEnumerable<Rune> runes, RuneLineTable lines)
        {
            foreach (var rune in runes)
            {
                lines.Append(rune);

                yield return rune;
            }
        }


        // Lines
        public static IEnumerable<TextSegment> GetLineSegments(this IEnumerable<Rune> values, LineEnding ending = null, bool withEnding = true)
        {
            return TextSegment.GetLineSegments(values, (c => (uint)c), ending, withEnding);
        }

        public static IEnumerable<(TextSegment Segment, RuneString Value)> GetLineData(this IEnumerable<Rune> values, LineEnding ending = null, bool withEnding = true)
        {
            return TextSegment.GetLineData(values, (c => (uint)c), new RuneBuffer(), (b => b.ToRuneString()), ending, withEnding);
        }

        public static IEnumerable<RuneString> GetLines(this IEnumerable<Rune> values, LineEnding ending = null, bool withEnding = true)
        {
            return TextSegment.GetLines(values, (c => (uint)c), new RuneBuffer(), (b => b.ToRuneString()), ending, withEnding);
        }

        public static IEnumerable<(TextSegment Segment, RuneString Value)> GetLineData(this RuneString values, LineEnding ending = null, bool withEnding = true)
        {
            return TextSegment.GetLineData<Rune, RuneString>(values, c => (uint)c, (s, p, l) => s.Extract(p, l), ending, withEnding);
        }

        public static IEnumerable<RuneString> GetLines(this RuneString values, LineEnding ending = null, bool withEnding = true)
        {
            return TextSegment.GetLines<Rune, RuneString>(values, c => (uint)c, (s, p, l) => s.Extract(p, l), ending, withEnding);
        }
    }
}