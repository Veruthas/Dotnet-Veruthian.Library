using Veruthian.Library.Types;
using Veruthian.Library.Text.Encodings;
using static Veruthian.Library.Text.Encodings.Utf32;
using System.Collections.Generic;
using System;

namespace Veruthian.Library.Text.Lines
{
    public sealed class LineEnding : Enum<LineEnding>
    {
        readonly string description;

        readonly string value;

        readonly int size;


        private LineEnding(string name, string description, string value, int size) : base(name)
        {
            this.description = description;

            this.value = value;

            this.size = size;
        }


        public string Description => description;

        public string Value => value;

        public int Size => size;


        public bool IsNewLine() => this != None;


        public static bool IsNewLine(uint value) => value == Utf32.Chars.Lf || value == Utf32.Chars.Cr;

        public static bool IsCarriageReturn(uint value) => value == Utf32.Chars.Cr;

        public static bool IsLineFeed(uint value) => value == Utf32.Chars.Lf;

        public static bool IsNewLine(uint value, uint next)
        {
            // \r
            if (value == Utf32.Chars.Cr)
                return next != Utf32.Chars.Lf; // only newline if not \r\n
            // \n
            else if (value == Utf32.Chars.Lf)
                return true;
            else
                return false;
        }


        public static LineEnding From(uint value)
        {
            switch (value)
            {
                case '\r':
                    return LineEnding.Cr;
                case '\n':
                    return LineEnding.Lf;
                default:
                    return LineEnding.None;
            }
        }

        public static LineEnding From(uint value, uint next)
        {
            if (value == Utf32.Chars.Cr)
                return next == Utf32.Chars.Lf ? LineEnding.CrLf : LineEnding.Cr;
            else if (value == Utf32.Chars.Lf)
                return LineEnding.Lf;
            else
                return LineEnding.None;
        }

        public static LineEnding From(LineEnding ending, uint next)
        {
            if (ending == LineEnding.Cr)
            {
                switch (next)
                {
                    case Utf32.Chars.Lf:
                        return LineEnding.CrLf;
                    default:
                        return LineEnding.None;
                }
            }
            else
            {
                switch (next)
                {
                    case Utf32.Chars.Lf:
                        return LineEnding.Lf;
                    case Utf32.Chars.Cr:
                        return LineEnding.Cr;
                    default:
                        return LineEnding.None;
                }
            }
        }


        public static LineEnding From(string value)
        {
            switch (value)
            {
                case "\r":
                    return LineEnding.Cr;
                case "\n":
                    return LineEnding.Lf;
                case "\r\n":
                    return LineEnding.CrLf;
                default:
                    return LineEnding.None;
            }
        }

        public static readonly LineEnding None = new LineEnding("None", "None or End of File", "", 0);

        public static readonly LineEnding Cr = new LineEnding("Cr", "Carriage Return", "\r", 1);

        public static readonly LineEnding Lf = new LineEnding("Lf", "Line Feed", "\n", 1);

        public static readonly LineEnding CrLf = new LineEnding("CrLf", "Carriage Return + Line Feed", "\r\n", 2);

        public static readonly LineEnding CarriageReturn = Cr;

        public static readonly LineEnding LineFeed = Lf;

        public static readonly LineEnding CarriageReturnLineFeed = CrLf;



        public static IEnumerable<(int Position, int Length, int LineNumber, LineEnding Ending)> GetLineSegments<U>(IEnumerable<U> values, Func<U, uint> getUtf32, LineEnding ending = null)
        {
            if (ending == null)
                ending = LineEnding.None;

            bool allowCrLf = ending == CrLf || ending == None;

            bool allowLf = ending == Lf || ending == None;

            bool allowCr = ending == Cr || ending == None;


            int lineNumber = 0;

            int position = 0;

            int length = 0;

            LineEnding found = None;


            foreach (var value in values)
            {
                uint utf32 = getUtf32(value);

                if (found == LineEnding.Cr)
                {
                    // CrLf
                    if (allowCrLf && utf32 == Utf32.Chars.Lf)
                    {
                        found = LineEnding.CrLf;

                        length++;

                        continue;
                    }
                    // Cr                    
                    else if (allowCr)
                    {
                        yield return (position, length, lineNumber++, found);

                        position += length;

                        length = 0;
                    }
                }
                // Lf
                else if (found == LineEnding.Lf)
                {
                    if (allowLf)
                    {
                        yield return (position, length, lineNumber++, found);

                        position += length;

                        length = 0;
                    }
                }
                // CrLf
                else if (found == LineEnding.CrLf)
                {
                    yield return (position, length, lineNumber++, found);

                    position += length;

                    length = 0;
                }


                length++;

                if (utf32 == Utf32.Chars.Cr)
                    found = LineEnding.Cr;
                else if (utf32 == Utf32.Chars.Lf)
                    found = LineEnding.Lf;
                else
                    found = LineEnding.None;

            }

            yield return (position, length, lineNumber++, found);

            if (found != LineEnding.None && (ending == LineEnding.None || ending == found))
                yield return (position, length, lineNumber++, found);
        }

        // Also should have a way to cache an actual IEnumerable<U>
        public static IEnumerable<S> GetLines<U, S>(S value, Func<U, uint> getUtf32, SliceText<S> slice, LineEnding ending = null, bool includeEnding = true)
            where S : IEnumerable<U>
        {
            foreach (var line in GetLineSegments(value, getUtf32, ending))
            {
                yield return slice(value, line.Position, line.Length);
            }
        }
    }
}