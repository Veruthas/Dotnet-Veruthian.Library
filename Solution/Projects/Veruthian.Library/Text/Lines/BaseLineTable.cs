using System;
using System.Collections;
using System.Collections.Generic;
using Veruthian.Library.Text.Encodings;
using Veruthian.Library.Text.Runes;

namespace Veruthian.Library.Text.Lines
{
    public abstract class BaseLineTable<U, S> : IEditableText<U, S>
        where S : IEnumerable<U>
    {
        List<TextSegment> segments;

        int length;

        LineEnding endingType;


        public BaseLineTable() : this(LineEnding.None) { }

        public BaseLineTable(LineEnding endingType)
        {
            this.segments = new List<TextSegment>();

            this.segments.Add((0, 0, 0, LineEnding.None));

            this.length = 0;

            this.endingType = endingType ?? LineEnding.None;
        }


        public int Count => segments[segments.Count - 1].Line + 1;

        public LineEnding EndingType => endingType;


        public TextSegment GetSegment(int line)
        {
            if (line < 0 || line >= Count)
                throw new ArgumentOutOfRangeException(nameof(line));

            if (endingType != LineEnding.CrLf)
            {
                var segment = this.segments[line];

                return segment;
            }
            else
            {
                var lineIndex = GetIndexFromNumber(line);

                var segments = GetLineSegments(lineIndex, line);

                return JoinSegments(segments);
            }
        }

        public TextSegment GetSegmentFromPosition(int position)
        {
            if (position < 0 && position > length)
                throw new ArgumentOutOfRangeException(nameof(position));

            if (endingType != LineEnding.CrLf)
            {
                return segments[GetLineFromPosition(position)];
            }
            else
            {
                var lineIndex = GetIndexFromPosition(position);

                var lineNumber = this.segments[lineIndex].Line;

                var segments = GetLineSegments(lineIndex, lineNumber);

                return JoinSegments(segments);
            }
        }

        public TextLocation GetTextLocation(int position)
        {
            var line = GetSegmentFromPosition(position);

            return new TextLocation(position, line.Line, position - line.Position);
        }

        public int GetLineFromPosition(int position)
        {
            var index = GetIndexFromPosition(position);

            return index != -1 ? segments[index].Line : -1;
        }


        // Helpers
        private TextSegment JoinSegments((int first, int last) segments)
        {
            var segment = this.segments[segments.first];

            for (int i = segments.first + 1; i <= segments.last; i++)
            {
                var nextSegment = this.segments[i];

                segment = (segment.Position, segment.Length + nextSegment.Length, segment.Line, nextSegment.Ending);
            }

            return segment;
        }

        private (int first, int last) GetLineSegments(int lineIndex, int lineNumber)
        {
            int first = lineIndex;

            int last = lineIndex;

            for (int i = lineIndex; i >= 0; i--)
            {
                if (segments[i].Line == lineNumber)
                    first = i;
                else
                    break;
            }
            for (int i = lineIndex; i < segments.Count; i++)
            {
                if (segments[i].Line == lineNumber)
                    last = i;
                else
                    break;
            }

            return (first, last);
        }

        private int GetIndexFromNumber(int lineNumber)
        {
            var low = 0;

            var high = segments.Count - 1;

            while (low <= high)
            {
                var middle = (low + high) / 2;

                var line = segments[middle];

                if (lineNumber < line.Line)
                    high = middle - 1;
                else if (lineNumber >= line.Line)
                    low = middle + 1;
                else
                    return middle;
            }

            return -1;
        }

        private int GetIndexFromPosition(int position)
        {
            var low = 0;

            var high = segments.Count - 1;

            while (low <= high)
            {
                var middle = (low + high) / 2;

                var line = segments[middle];

                if (position < line.Position)
                    high = middle - 1;
                else if (position >= line.Position + line.Length)
                    low = middle + 1;
                else
                    return middle;
            }

            return -1;
        }

        private void Adjust(int lineOffset, int positionOffset, int index)
        {
            for (int i = index; i < segments.Count; i++)
            {
                var line = segments[i];

                line.Position += positionOffset;

                line.Line += lineOffset;

                segments[i] = line;
            }
        }

        private int NewLineOffset(LineEnding ending) => (endingType != LineEnding.CrLf || ending == LineEnding.CrLf ? 1 : 0);


        // Append
        public void Append(U value) => Append(false, default(S), value);

        public void Append(S values) => Append(true, values, default(U));

        private void Append(bool enumerate, S values, U value)
        {
            var segment = segments[segments.Count - 1];

            var added = 0;

            void UpdateSegment()
            {
                segments[segments.Count - 1] = segment;

                if (segment.Ending != LineEnding.None)
                {
                    var newlineOffset = NewLineOffset(segment.Ending);

                    var lineNumber = segment.Line + newlineOffset;

                    segment = (lineNumber, segment.Position + segment.Length, 0, LineEnding.None);

                    segments.Add(segment);
                }
            }

            void ProcessItem(U item)
            {
                var utf32 = ConvertToUtf32(item);

                // Cr
                if (endingType != LineEnding.Lf && LineEnding.IsCarriageReturn(utf32))
                {
                    if (segment.Ending != LineEnding.None)
                        UpdateSegment();

                    segment.Ending = LineEnding.Cr;

                    segment.Length++;
                }
                // Lf
                else if (endingType != LineEnding.Cr && LineEnding.IsLineFeed(utf32))
                {
                    // If allows CrLf
                    if (endingType != LineEnding.Lf)
                    {
                        // If <...Lf> + <Cr>
                        if (segment.Ending == LineEnding.Cr)
                        {
                            segment.Length++;

                            segment.Ending = LineEnding.CrLf;
                        }
                        // On <...Lf> + <Cr> when first character of entire append is Cr
                        else if (segment.Length == 0 && segments.Count > 1 && segments[segments.Count - 2].Ending == LineEnding.Cr)
                        {
                            var lastsegment = segments[segments.Count - 2];

                            lastsegment.Length++;

                            lastsegment.Ending = LineEnding.CrLf;

                            segments[segments.Count - 2] = lastsegment;

                            segment.Position++;
                        }
                        // Lf
                        else
                        {
                            if (segment.Ending != LineEnding.None)
                                UpdateSegment();

                            segment.Ending = LineEnding.Lf;

                            segment.Length++;
                        }
                    }
                    // Lf
                    else
                    {
                        if (segment.Ending != LineEnding.None)
                            UpdateSegment();

                        segment.Ending = LineEnding.Lf;

                        segment.Length++;
                    }
                }
                // None
                else
                {
                    if (segment.Ending != LineEnding.None)
                        UpdateSegment();

                    segment.Length++;
                }

                added++;
            }

            if (enumerate)
                foreach (var item in values)
                    ProcessItem(item);
            else
                ProcessItem(value);

            UpdateSegment();

            length += added;
        }


        // Prepend
        public void Prepend(U value) => Prepend(false, default(S), value);

        public void Prepend(S values) => Prepend(true, values, default(U));

        private void Prepend(bool enumerate, S values, U value)
        {
            if (length == 0)
                Append(enumerate, values, value);
            else
                Insert(enumerate, 0, 0, values, value);
        }


        // Insert
        public void Insert(int position, U value) => RoutedInsert(false, position, default(S), value);

        public void Insert(int position, S values) => RoutedInsert(true, position, values, default(U));

        private void RoutedInsert(bool enumerate, int position, S values, U value)
        {
            if (position == length)
                Append(enumerate, values, value);
            else if (position < 0 || position > length)
                throw new ArgumentOutOfRangeException(nameof(position));
            else
                Insert(enumerate, position, values, value);
        }

        private void Insert(bool enumerate, int position, S values, U value)
        {
            var index = GetIndexFromPosition(position);

            var segment = this.segments[index];

            var column = position - segment.Position;

            Insert(enumerate, index, column, values, value);
        }

        private void Insert(bool enumerate, int index, int column, S values, U value)
        {
            var segment = this.segments[index];

            var lastsegment = index > 0 ? this.segments[index - 1] : (0, 0, 0, LineEnding.None);

            var postponedLf = false;

            var columnOffset = 0;

            var lineOffset = 0;

            var positionOffset = 0;


            void SplitSegment(LineEnding ending)
            {
                var newlineOffset = NewLineOffset(ending);


                lastsegment = (segment.Position, column + columnOffset, segment.Line, ending);

                segment = (segment.Position + lastsegment.Length, segment.Length - lastsegment.Length, segment.Line + newlineOffset, segment.Ending);


                segments[index++] = lastsegment;

                segments.Insert(index, segment);


                lineOffset += newlineOffset;

                positionOffset += columnOffset;

                column = columnOffset = 0;
            }

            void ProcessItem(U item)
            {
                var utf32 = ConvertToUtf32(item);

                // Cr
                if (endingType != LineEnding.Lf && LineEnding.IsCarriageReturn(utf32))
                {
                    segment.Length++;

                    columnOffset++;

                    SplitSegment(LineEnding.Cr);
                }
                // Lf
                else if (endingType != LineEnding.Cr && LineEnding.IsLineFeed(utf32))
                {
                    // CrLf <..Cr> + [Lf]
                    if (endingType != LineEnding.Lf && lastsegment.Ending == LineEnding.Cr && column + columnOffset == 0)
                    {
                        lastsegment.Length++;

                        lastsegment.Ending = LineEnding.CrLf;

                        segments[index - 1] = lastsegment;

                        segment.Position++;

                        positionOffset++;

                        if (endingType == LineEnding.CrLf)
                        {
                            segment.Line++;

                            lineOffset++;
                        }
                    }
                    // Lf
                    else
                    {
                        segment.Length++;

                        columnOffset++;

                        SplitSegment(LineEnding.Lf);
                    }
                }
                // None
                else
                {
                    segment.Length++;

                    columnOffset++;
                }
            }


            void ProcessInitial()
            {
                // Push off <...[?]Lf> processing until end
                if (column == segment.Length - 1)
                {
                    // Split a <..Lf> into <..> + {Lf}
                    if (segment.Ending == LineEnding.Lf && endingType != LineEnding.Lf)
                    {
                        segment.Ending = LineEnding.None;

                        segment.Length--;

                        postponedLf = true;
                    }
                    // Split a <..CrLf> into <..Cr> + <> + {Lf}
                    else if (segment.Ending == LineEnding.CrLf)
                    {
                        // <..Cr>|{Lf}                        
                        segment.Ending = LineEnding.Cr;

                        segment.Length--;

                        lastsegment = segment;

                        segments[index++] = lastsegment;


                        // Add new empty segment
                        var newlineOffset = NewLineOffset(LineEnding.Cr);

                        segment = (segment.Position + segment.Length, 0, segment.Line + newlineOffset, LineEnding.None);

                        segments.Insert(index, segment);

                        column = columnOffset = 0;


                        // If Cr is not considered a line
                        lineOffset += newlineOffset;

                        postponedLf = true;
                    }
                }
            }

            void ProcessFinal()
            {
                if (postponedLf)
                {
                    // <...?> + <> + {Lf}
                    if (segment.Length == 0)
                    {
                        // <...Cr> + <> + {Lf} & endingType = CrLf | None
                        if (lastsegment.Ending == LineEnding.Cr && (endingType == LineEnding.CrLf || endingType == LineEnding.None))
                        {
                            segments.RemoveAt(index);

                            lastsegment.Length++;

                            lastsegment.Ending = LineEnding.CrLf;

                            segments[index - 1] = lastsegment;

                            // Increment line if Cr is not a newline
                            lineOffset -= (NewLineOffset(LineEnding.Cr));
                        }
                        // <...?> + <{Lf}>
                        else
                        {
                            segment.Length = 1;

                            segment.Ending = LineEnding.Lf;

                            segments[index++] = segment;
                        }
                    }
                    // <...None> + {Lf}
                    else if (segment.Ending == LineEnding.None)
                    {
                        segment.Length++;

                        segment.Ending = LineEnding.Lf;

                        segments[index++] = segment;
                    }
                    // <...Cr> + {Lf} & endingType = CrLf | None
                    else if (segment.Ending == LineEnding.Cr && (endingType == LineEnding.CrLf || endingType == LineEnding.None))
                    {
                        segment.Length++;

                        segment.Ending = LineEnding.CrLf;

                        segments[index++] = segment;

                        // Increment line if Cr is not a newline
                        lineOffset -= (NewLineOffset(LineEnding.Cr));
                    }
                    // <...?> + <{Lf}>
                    else
                    {
                        segments[index++] = segment;

                        segment = (segment.Position + segment.Length, 1, segment.Line, LineEnding.Lf);

                        segments.Insert(index++, segment);
                    }
                }
                // Otherwise just update the last segment
                else
                {
                    segments[index++] = segment;
                }

                positionOffset += columnOffset;
            }


            ProcessInitial();

            if (enumerate)
                foreach (var item in values)
                    ProcessItem(item);
            else
                ProcessItem(value);

            ProcessFinal();


            if (positionOffset != 0)
                Adjust(lineOffset, positionOffset, index);

            length += positionOffset;
        }


        // Remove
        public void Remove(int position, int amount)
        {
            if (position < 0 || position > length)
                throw new ArgumentOutOfRangeException(nameof(position));

            if (amount < 0 || position + amount > length)
                throw new ArgumentOutOfRangeException(nameof(amount));


            var index = GetIndexFromPosition(position);

            var segment = segments[index];

            var column = position - segment.Position;


            var segmentCount = 0;


            var lineOffset = 0;

            var positionOffset = amount;


            var lastLineNumber = segment.Line;


            // If removing within a segment
            if (column + amount < segment.Length)
            {
                segment.Length -= amount;

                segments[index] = segment;

                Adjust(0, amount, index + 1);
            }
            // If removing one or more segments
            else
            {
                var dangling = segment;

                // If removing end of first segment
                if (column != 0)
                {
                    dangling.Length -= column;

                    dangling.Ending = LineEnding.None;

                    amount -= dangling.Length;

                    lineOffset++;

                    index++;
                }

                // If removing rest of segments
                if (position + amount == length)
                {
                    segments.RemoveRange(index, segments.Count - index);

                    if (column != 0)
                        segments[index - 1] = dangling;
                }
                // Otherwise            
                else
                {

                    while (amount > 0)
                    {
                        segment = segments[index + segmentCount];

                        // If entire segment is being removed
                        if (segment.Length >= amount)
                        {
                            amount -= segment.Length;

                            segment.Length = 0;

                            segmentCount++;

                            // Increment LineOffset for adjustment
                            if (segment.Line != lastLineNumber)
                            {
                                lineOffset++;

                                lastLineNumber = segment.Line;
                            }
                        }
                        // If removing beginning of last segment 
                        else
                        {
                            segment.Position += amount;

                            segment.Length -= amount;
                        }
                    }

                    // Need to merge in dangling
                    if (column != 0)
                    {
                        // If removed entire last segment, get next for dangling
                        if (segment.Length == 0)
                            segment = segments[index + segmentCount + 1];

                        dangling.Length += segment.Length;

                        dangling.Ending = segment.Ending;

                        segments[index - 1] = dangling;
                    }
                    // If removed only beginning of last segment
                    else if (segment.Length != 0)
                    {
                        segments[index + segmentCount] = segment;
                    }

                    // Remove and adjust
                    segments.RemoveRange(index, segmentCount);

                    Adjust(lineOffset, positionOffset, index);
                }
            }
        }

        // Clear
        public void Clear()
        {
            segments.Clear();
            length = 0;
        }

        // Enumerator
        public IEnumerable<TextSegment> Lines
        {
            get
            {
                if (endingType != LineEnding.CrLf)
                {
                    foreach (var line in segments)
                        yield return line;
                }
                else
                {
                    var lineNumber = 0;

                    var segment = segments[0];

                    for (int i = 1; i < segments.Count; i++)
                    {
                        var nextSegment = segments[i];

                        if (nextSegment.Line == lineNumber)
                        {
                            segment.Length += nextSegment.Length;

                            segment.Ending = nextSegment.Ending;
                        }
                        else
                        {
                            yield return segment;

                            lineNumber++;

                            segment = nextSegment;
                        }
                    }

                    yield return segment;
                }
            }
        }


        // Abstract
        protected abstract uint ConvertToUtf32(U value);
    }
}