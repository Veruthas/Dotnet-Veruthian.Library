using System;
using System.Collections;
using System.Collections.Generic;
using Veruthian.Library.Text.Encodings;
using Veruthian.Library.Text.Runes;

namespace Veruthian.Library.Text.Lines
{
    public abstract class BaseLineTable<I, S> : IEnumerable<(int LineNumber, int Position, int Length, LineEnding Ending)>
        where S : IEnumerable<I>
    {

        private struct LineSegment
        {
            public int LineNumber;

            public int Position;

            public int Length;

            public LineEnding Ending;


            public LineSegment(int lineNumber, int position, int length, LineEnding ending)
            {
                this.LineNumber = lineNumber;
                this.Position = position;
                this.Length = length;
                this.Ending = ending;
            }

            public (int LineNumber, int Position, int Length, LineEnding Ending) ToTuple()
            {
                return (LineNumber, Position, Length, Ending);
            }



            public static implicit operator (int LineNumber, int Position, int Length, LineEnding Ending) (LineSegment value)
            {
                return value.ToTuple();
            }

            public static implicit operator LineSegment((int LineNumber, int Position, int Length, LineEnding Ending) value)
            {
                return new LineSegment(value.LineNumber, value.Position, value.Length, value.Ending);
            }


            public override string ToString()
            {
                return $"{nameof(LineNumber)}: {LineNumber}; {nameof(Position)}: {Position}; {nameof(Length)}: {Length}; {nameof(Ending)}: {Ending};";
            }
        }


        List<LineSegment> lines;

        int length;

        LineEnding endingType;


        public BaseLineTable() : this(LineEnding.None) { }

        public BaseLineTable(LineEnding endingType)
        {
            this.lines = new List<LineSegment>();

            this.lines.Add((0, 0, 0, LineEnding.None));

            this.length = 0;

            this.endingType = endingType;
        }


        public int Count => lines[lines.Count - 1].LineNumber + 1;


        public LineEnding EndingType => LineEnding.None;

        private bool AllowsCrLf => endingType == LineEnding.None || endingType == LineEnding.CrLf;



        public (int LineNumber, int Position, int Length, LineEnding Ending) GetLine(int lineNumber)
        {
            if (lineNumber < 0 || lineNumber >= Count)
                throw new ArgumentOutOfRangeException(nameof(lineNumber));

            if (endingType == LineEnding.None)
            {
                var line = lines[lineNumber];

                return line;
            }
            else
            {
                var lineIndex = GetLineIndexFromNumber(lineNumber);

                var segments = GetLineSegments(lineIndex, lineNumber);

                return JoinSegments(segments);
            }
        }

        public (int LineNumber, int Position, int Length, LineEnding Ending) GetLineFromPosition(int position)
        {
            if (position < 0 && position > length)
                throw new ArgumentOutOfRangeException(nameof(position));

            if (endingType == LineEnding.None)
            {
                return lines[GetLineNumber(position)];
            }
            else
            {
                var lineIndex = GetLineIndexFromPosition(position);

                var lineNumber = lines[lineIndex].LineNumber;

                var segments = GetLineSegments(lineIndex, lineNumber);

                return JoinSegments(segments);
            }
        }

        public TextLocation GetTextLocation(int position)
        {
            var line = GetLineFromPosition(position);

            return new TextLocation(position, line.LineNumber, position - line.Position);
        }

        public int GetLineNumber(int position)
        {
            var index = GetLineIndexFromPosition(position);

            return index != -1 ? lines[index].LineNumber : -1;
        }

        private bool MatchesType(LineEnding matcher, LineEnding value)
        {
            return matcher == LineEnding.None || matcher == value;
        }

        private LineSegment JoinSegments((int first, int last) segments)
        {
            var segment = lines[segments.first];

            for (int i = segments.first + 1; i <= segments.last; i++)
            {
                var nextSegment = lines[i];

                segment = (segment.LineNumber, segment.Position, segment.Length + nextSegment.Length, nextSegment.Ending);
            }

            return segment;
        }

        private (int first, int last) GetLineSegments(int lineIndex, int lineNumber)
        {
            int first = lineIndex;

            int last = lineIndex;

            for (int i = lineIndex; i >= 0; i--)
            {
                if (lines[i].LineNumber == lineNumber)
                    first = i;
                else
                    break;
            }
            for (int i = lineIndex; i < lines.Count; i++)
            {
                if (lines[i].LineNumber == lineNumber)
                    last = i;
                else
                    break;
            }

            return (first, last);
        }

        private int GetLineIndexFromNumber(int lineNumber)
        {
            var low = 0;

            var high = lines.Count - 1;

            while (low <= high)
            {
                var middle = (low + high) / 2;

                var line = lines[middle];

                if (lineNumber < line.LineNumber)
                    high = middle - 1;
                else if (lineNumber >= line.LineNumber)
                    low = middle + 1;
                else
                    return middle;
            }

            return -1;
        }

        private int GetLineIndexFromPosition(int position)
        {
            var low = 0;

            var high = lines.Count - 1;

            while (low <= high)
            {
                var middle = (low + high) / 2;

                var line = lines[middle];

                if (position < line.Position)
                    high = middle - 1;
                else if (position >= line.Position + line.Length)
                    low = middle + 1;
                else
                    return middle;
            }

            return -1;
        }


        private void Adjust(int positionOffset, int lineOffset, int start)
        {
            for (int i = start; i < lines.Count; i++)
            {
                var line = lines[i];

                line.Position += positionOffset;

                line.LineNumber += lineOffset;

                lines[i] = line;
            }

            length += positionOffset;
        }


        public void Prepend(I value)
        {
            var line = lines[0];

            var utf32 = ConvertToUtf32(value);

            // Special Case: lines[0] = (0, 1, Lf) and value == Cr and endingType = CrLf|None
            if (LineEnding.IsCarriageReturn(utf32))
            {
                // CrLf                
                if ((line.Length == 1) && (line.Ending == LineEnding.Lf) && (endingType == LineEnding.None || endingType == LineEnding.CrLf))
                {
                    line.Length++;

                    line.Ending = LineEnding.CrLf;

                    lines[0] = line;

                    Adjust(1, 0, 1);
                }
                // Cr
                else
                {
                    var newline = (0, 0, 1, LineEnding.Cr);

                    lines.Insert(0, newline);

                    var lineOffset = endingType == LineEnding.Cr || endingType == LineEnding.None ? 1 : 0;

                    Adjust(1, lineOffset, 1);
                }
            }
            // Lf
            else if (LineEnding.IsLineFeed(utf32))
            {
                var newline = (0, 0, 1, LineEnding.Lf);

                lines.Insert(0, newline);

                var lineOffset = endingType == LineEnding.Lf || endingType == LineEnding.None ? 1 : 0;

                Adjust(1, lineOffset, 1);
            }
            else
            {
                line.Length++;

                lines[0] = line;

                Adjust(1, 0, 1);
            }
        }

        public void Prepend(IEnumerable<I> values)
        {
        }

        private void Prepend(IEnumerable<I> values, I value)
        {
            var segment = new LineSegment(0, 0, 0, LineEnding.None);

            int lineOffset = 0;

            int positionOffset = 0;

            int segments = 0;


            void InsertSegment()
            {
                positionOffset += segment.Length;

                if (MatchesType(endingType, segment.Ending))
                    lineOffset++;

                lines.Insert(segments++, segment);

                segment = (lineOffset, positionOffset, 0, LineEnding.None);
            }

            void ProcessItem(I item)
            {
                var utf32 = ConvertToUtf32(value);

                // Cr
                if (LineEnding.IsCarriageReturn(utf32))
                {
                    if (segment.Ending != LineEnding.None)
                        InsertSegment();

                    segment.Ending = LineEnding.Cr;

                    segment.Length++;
                }
                // Lf
                else if (LineEnding.IsLineFeed(utf32))
                {
                    // CrLf
                    if (segment.Ending == LineEnding.Cr && AllowsCrLf)
                    {
                        segment.Ending = LineEnding.CrLf;

                        segment.Length++;
                    }
                    else
                    {
                        if (segment.Ending != LineEnding.None)
                            InsertSegment();

                        segment.Ending = LineEnding.Lf;

                        segment.Length++;
                    }
                }
                else
                {
                    if (segment.Ending != LineEnding.None)
                        InsertSegment();

                    segment.Length++;
                }
            }

            void ProcessFinal()
            {
                if (segment.Length != 0)
                {
                    var nextline = lines[segments];

                    if (segment.Ending.IsNewLine())
                    {
                        // Merge on special case of <....Cr> + <Lf>
                        if (AllowsCrLf && segment.Ending == LineEnding.Cr && nextline.Length == 1 && nextline.Ending == LineEnding.Lf)
                        {
                            nextline.Position = segment.Position;

                            nextline.Length += segment.Length;

                            nextline.Ending = LineEnding.CrLf;

                            lines[segments] = nextline;
                        }
                        else
                        {
                            InsertSegment();
                        }
                    }
                    // Merge
                    else
                    {
                        nextline.Length += segment.Length;

                        lines[segments] = nextline;
                    }
                }
            }


            if (values != null)
                foreach (var item in values)
                    ProcessItem(item);
            else
                ProcessItem(value);


            ProcessFinal();

            if (segments != 0)
                Adjust(positionOffset, lineOffset, segments);
        }

        public void Append(I value)
        {
            // Special Case: #lines > 1 and lines[last - 2].Ending = Cr and value = Lf
        }

        public void Append(IEnumerable<I> values)
        {
            // Special Case: #lines > 1 and lines[last - 2].Ending = Cr and value.First = Lf
        }


        public void Insert(int position, I value)
        {
            if (position == 0)
                Prepend(value);
            else if (position == length)
                Append(value);
            else if (position < 0 || position > length)
                throw new ArgumentOutOfRangeException(nameof(position));
            else
            {
                var lineNumber = GetLineNumber(position);
            }
        }

        public void Insert(int position, IEnumerable<I> values)
        {
            if (position == 0)
                Prepend(values);
            else if (position == length)
                Append(values);
            else if (position < 0 || position > length)
                throw new ArgumentOutOfRangeException(nameof(position));
            else
            {
                var lineNumber = GetLineNumber(position);
            }
        }


        public void Remove(int position, int amount)
        {
            if (position < 0 || position > length)
                throw new ArgumentOutOfRangeException(nameof(position));

            if (amount < 0 || position + amount > length)
                throw new ArgumentOutOfRangeException(nameof(amount));

            var lineNumber = GetLineNumber(position);
        }


        private S ExtractLine(S value, int start, int length)
        {
            var valueLength = GetLength(value);

            if (start <= valueLength)
            {
                var end = start + length;

                if (end > valueLength)
                    end = valueLength - start;

                return Slice(value, start, length);
            }
            else
            {
                return default(S);
            }
        }

        public S ExtractLine(S value, int lineNumber, bool includeEnd = true)
        {
            if (lineNumber < 0 || lineNumber >= Count)
                throw new ArgumentOutOfRangeException(nameof(lineNumber));

            var line = GetLine(lineNumber);

            return ExtractLine(value, line.Position, line.Length - (includeEnd ? 0 : line.Ending.Size));
        }

        public IEnumerable<S> ExtractLines(S value, bool includeEnd = true)
        {
            foreach (var line in this)
                yield return ExtractLine(value, line.Position, line.Length - (includeEnd ? 0 : line.Ending.Size));
        }



        public IEnumerator<(int LineNumber, int Position, int Length, LineEnding Ending)> GetEnumerator()
        {
            if (endingType == LineEnding.None)
            {
                foreach (var line in lines)
                    yield return line;
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();


        protected abstract uint ConvertToUtf32(I value);

        protected abstract int GetLength(S value);

        protected abstract S Slice(S value, int start, int length);
    }
}