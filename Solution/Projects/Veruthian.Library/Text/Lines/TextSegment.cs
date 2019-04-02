using System;
using System.Collections.Generic;
using Veruthian.Library.Text.Encodings;

namespace Veruthian.Library.Text.Lines
{
    public struct TextSegment : IEquatable<TextSegment>
    {
        int position;

        int length;

        int line;

        LineEnding ending;



        public TextSegment(int position, int length) : this(position, length, 0, LineEnding.None) { }

        public TextSegment(int position, int length, int line, LineEnding ending)
        {
            this.position = position;

            this.length = length;

            this.line = line;

            this.ending = ending;
        }


        public int Position { get => position; set => position = value; }

        public int Length { get => length; set => length = value; }

        public int Line { get => line; set => line = value; }

        public LineEnding Ending { get => ending ?? LineEnding.None; set => ending = value; }


        public void Deconstruct(out int position, out int length, out int line, out LineEnding ending)
        {
            position = this.Position;

            length = this.Length;

            line = this.Line;

            ending = this.Ending;
        }

        public void Deconstruct(out int position, out int length)
        {
            position = this.Position;

            length = this.Length;
        }


        public S Extract<S>(S value, ExtractText<S> extractor) => extractor(value, position, length);
        

        public (int Position, int Length, int Line, LineEnding Ending) ToTuple()
        {
            return (Position, Length, Line, Ending);
        }

        public static implicit operator TextSegment((int Position, int Length, int Line, LineEnding Ending) segment)
            => new TextSegment(segment.Position, segment.Length, segment.Line, segment.Ending);


        public override int GetHashCode() => Utility.HashCodes.Default.Combine(Position, Length, Line, Ending);


        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is TextSegment))
                return false;

            return this.Equals((TextSegment)obj);
        }

        public bool Equals(TextSegment other)
        {
            return this.position == other.position && this.length == other.length && this.line == other.line && this.ending == other.ending;
        }


        public override string ToString() => $"{nameof(Position)}: {Position}; {nameof(Length)}: {Length}; {nameof(Line)}: {Line}; {nameof(Ending)}: {Ending};";



        public static IEnumerable<TextSegment> GetLineSegments<U>(IEnumerable<U> values, Func<U, uint> getUtf32, LineEnding ending = null,
                                                                  bool withEnding = true, IEditableText<U> buffer = null)
        {
            if (ending == null)
                ending = LineEnding.None;

            bool allowCrLf = ending == LineEnding.CrLf || ending == LineEnding.None;

            bool allowLf = ending == LineEnding.Lf || ending == LineEnding.None;

            bool allowCr = ending == LineEnding.Cr || ending == LineEnding.None;


            int lineNumber = 0;

            int position = 0;

            int length = 0;

            LineEnding found = LineEnding.None;


            foreach (var value in values)
            {
                uint utf32 = getUtf32(value);

                if (found == LineEnding.Cr)
                {
                    // CrLf
                    if (allowCrLf && utf32 == Utf32.Chars.Lf)
                    {
                        found = LineEnding.CrLf;

                        if (buffer != null && withEnding)
                            buffer.Append(value);

                        length++;

                        continue;
                    }
                    // Cr                    
                    else if (allowCr)
                    {
                        yield return (position, length - (withEnding ? 0 : found.Size), lineNumber++, found);

                        position += length;

                        length = 0;
                    }
                }
                // Lf
                else if (found == LineEnding.Lf)
                {
                    if (allowLf)
                    {
                        yield return (position, length - (withEnding ? 0 : found.Size), lineNumber++, found);

                        position += length;

                        length = 0;
                    }
                }
                // CrLf
                else if (found == LineEnding.CrLf)
                {
                    yield return (position, length - (withEnding ? 0 : found.Size), lineNumber++, found);

                    position += length;

                    length = 0;
                }


                length++;

                if (utf32 == Utf32.Chars.Cr)
                {
                    if (buffer != null && withEnding)
                        buffer.Append(value);

                    found = LineEnding.Cr;
                }
                else if (utf32 == Utf32.Chars.Lf)
                {
                    if (buffer != null && withEnding)
                        buffer.Append(value);

                    found = LineEnding.Lf;
                }
                else
                {
                    if (buffer != null)
                        buffer.Append(value);

                    found = LineEnding.None;
                }
            }

            yield return (position, length - (withEnding ? 0 : found.Size), lineNumber++, found);

            if (found != LineEnding.None && (ending == LineEnding.None || ending == found))
                yield return (position, 0, lineNumber, LineEnding.None);
        }

        public static IEnumerable<S> GetExtractedLines<U, S>(S values, Func<U, uint> getUtf32, ExtractText<S> extractor, LineEnding ending = null, bool withEnding = true)
            where S : IEnumerable<U>
        {
            foreach (var line in GetLineSegments(values, getUtf32, ending))
            {
                yield return extractor(values, line.position, line.length);
            }
        }


        public static IEnumerable<S> GetBufferedLines<U, S, B>(IEnumerable<U> values, Func<U, uint> getUtf32,
                                                               B buffer, Func<B, S> getBufferItem,
                                                               LineEnding ending = null, bool withEnding = true)
            where B : IEditableText<U>
        {
            foreach(var line in GetLineSegments(values, getUtf32, ending, withEnding, buffer))
            {
                yield return getBufferItem(buffer);

                buffer.Clear();
            }
        }


        public static IEnumerable<(TextSegment, S)> GetBufferedLineSegments<U, S, B>(IEnumerable<U> values, Func<U, uint> getUtf32,
                                                               B buffer, Func<B, S> getBufferItem,
                                                               LineEnding ending = null, bool withEnding = true)
            where B : IEditableText<U>
        {
            foreach (var line in GetLineSegments(values, getUtf32, ending, withEnding, buffer))
            {
                yield return (line, getBufferItem(buffer));

                buffer.Clear();
            }
        }

    }
}