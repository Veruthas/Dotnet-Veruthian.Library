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

            this.endingType = endingType ?? LineEnding.None;

            Clear();
        }


        public int Count => segments[segments.Count - 1].Line + 1;

        public LineEnding Ending => endingType;


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
                var segmentIndex = GetIndexFromLine(line);

                segmentIndex = GetFirstSegment(segmentIndex, line);

                return JoinSegments(segmentIndex, line);
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
                var segmentIndex = GetIndexFromPosition(position);

                var lineNumber = this.segments[segmentIndex].Line;

                segmentIndex = GetFirstSegment(segmentIndex, lineNumber);

                return JoinSegments(segmentIndex, lineNumber);
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
        private TextSegment JoinSegments(int segmentIndex, int lineNumber)
        {
            var segment = this.segments[segmentIndex];

            for (int i = segmentIndex + 1; i < segments.Count; i++)
            {
                var nextSegment = this.segments[i];

                if (nextSegment.Line != lineNumber)
                    break;

                segment.Length += nextSegment.Length;

                segment.Ending = nextSegment.Ending;
            }

            return segment;
        }

        private int GetFirstSegment(int segmentIndex, int lineNumber)
        {
            for (; segmentIndex >= 0; segmentIndex--)
            {
                if (segments[segmentIndex].Line != lineNumber)
                    break;
            }

            return segmentIndex;
        }

        private int GetIndexFromLine(int lineNumber)
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

        private int NewLineOffset(LineEnding ending)
        {
            // Either it accepts them all or only its own type
            return (endingType == LineEnding.None || ending == endingType) ? 1 : 0;
        }


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

                    segment = (segment.Position + segment.Length, 0, lineNumber, LineEnding.None);

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
                        // If <...Cr> + [Lf]
                        if (segment.Ending == LineEnding.Cr)
                        {
                            segment.Length++;

                            segment.Ending = LineEnding.CrLf;
                        }
                        // On <...Cr> + [Lf] when first character of entire append is Lf
                        else if (segment.Length == 0 && segments.Count > 1 && segments[segments.Count - 2].Ending == LineEnding.Cr)
                        {
                            var lastsegment = segments[segments.Count - 2];

                            lastsegment.Length++;

                            lastsegment.Ending = LineEnding.CrLf;

                            segments[segments.Count - 2] = lastsegment;

                            segment.Position++;

                            // If Cr didn't count as a line ending, increment current segment line number
                            if (endingType == LineEnding.CrLf)
                                segment.Line++;
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
                //         NONE
                // <(n)...{Lf
                //     }>  <(n + 1)...>

                // <(n)...>      <(n + 1)...>
                //         {Lf
                // }


                //          CrLf
                // <(n)...{Lf}>   <(n)...>

                // <(n)...>       <(n)...>
                //           {Lf}



                //         NONE
                // <(n)...{CrLf}>  <(n + 1)...>
                //          offset = 0              
                // <(n)...{Cr}>  <(n+1)>  <(n+1+offset=n+1)...>
                //             {Lf}


                //         CrLf
                // <(n)...{CrLf}>  <(n + 1)...>

                //         offset = -1
                // <(n)...{Cr}> <(n)> <(n+1+offset=n)...>
                //             {Lf}


                // Push off <...[?]Lf> processing until end
                if (endingType == LineEnding.None || endingType == LineEnding.CrLf)
                {
                    if (column == segment.Length - 1)
                    {
                        // Split a <..Lf> into <..> + {Lf}
                        if (segment.Ending == LineEnding.Lf)
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
                            var newlineOffset = endingType == LineEnding.None ? 1 : 0;

                            segment = (segment.Position + segment.Length, 0, segment.Line + newlineOffset, LineEnding.None);

                            segments.Insert(index, segment);

                            column = columnOffset = 0;


                            // If Cr is not considered a line
                            if (endingType == LineEnding.CrLf)
                                lineOffset--;

                            postponedLf = true;
                        }
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
                        // <...Cr> + <> + {Lf}
                        if (lastsegment.Ending == LineEnding.Cr)
                        {
                            segments.RemoveAt(index);

                            lastsegment.Length++;

                            lastsegment.Ending = LineEnding.CrLf;

                            segments[index - 1] = lastsegment;

                            // Increment line if Cr was not a newline, otherwise cancel out previous addition of char
                            lineOffset += (endingType == LineEnding.CrLf) ? 1 : -1;
                        }
                        // <...?> + <{Lf}>
                        else
                        {
                            segment.Length = 1;

                            segment.Ending = LineEnding.Lf;

                            segments[index++] = segment;

                            if (endingType == LineEnding.None)
                                lineOffset++;
                        }
                    }
                    // <...None> + {Lf}
                    else if (segment.Ending == LineEnding.None)
                    {
                        segment.Length++;

                        segment.Ending = LineEnding.Lf;

                        segments[index++] = segment;
                    }
                    // <...Cr> + {Lf}
                    else if (segment.Ending == LineEnding.Cr)
                    {
                        segment.Length++;

                        segment.Ending = LineEnding.CrLf;

                        segments[index++] = segment;

                        // Increment line if Cr was not a newline
                        if (endingType == LineEnding.CrLf)
                            lineOffset++;
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

            if (amount == 0)
            {
                return;
            }
            else if (amount == length)
            {
                Clear();
            }
            else
            {
                var positionOffset = -amount;

                var lineOffset = 0;


                var index = GetIndexFromPosition(position);


                var startIndex = index;

                var startSegment = segments[index];

                var startColumn = position - startSegment.Position;


                // Within one segment
                if (startColumn + amount < startSegment.Length - (startSegment.Ending == LineEnding.CrLf ? 2 : 1))
                {
                    startSegment.Length -= amount;

                    segments[startIndex] = startSegment;

                    Adjust(lineOffset, positionOffset, index + 1);
                }
                // Over multiple segment
                else
                {
                    var segmentCount = 0;

                    var lastLineNumber = startSegment.Line;


                    // Handle first segment
                    if (startColumn == 0)
                    {
                        startIndex = index - 1;

                        if (index != -1)
                        {
                            startSegment = segments[startIndex];

                            startColumn = startSegment.Length;
                        }
                    }
                    else
                    {
                        if (startSegment.Ending == LineEnding.CrLf && startColumn == startSegment.Length - 1)
                        {
                            startSegment.Ending = LineEnding.Cr;

                            if (endingType == LineEnding.CrLf)
                                lineOffset--;
                        }
                        else
                        {
                            startSegment.Ending = LineEnding.None;

                            lineOffset--;
                        }

                        amount -= startSegment.Length - startColumn;

                        startSegment.Length = startColumn;

                        index++;
                    }

                    // Handle middle segments
                    while (index < segments.Count)
                    {
                        var segment = segments[index];

                        if (segment.Length <= amount)
                        {
                            if (segment.Line != lastLineNumber)
                                lineOffset++;

                            lastLineNumber = segment.Line;

                            amount -= segment.Length;

                            segmentCount++;

                            index++;
                        }
                        else
                        {
                            break;
                        }
                    }

                    // Handle last segment
                    if (index < segments.Count)
                    {
                        var lastSegment = segments[index];

                        var lastColumn = amount;

                        lastSegment.Position += amount;

                        lastSegment.Length -= amount;

                        if (lastSegment.Length == 1 && lastSegment.Ending == LineEnding.CrLf)
                        {
                            lastSegment.Ending = LineEnding.Lf;

                            if (endingType == LineEnding.CrLf)
                                lineOffset--;
                        }

                        // Try to combine first and last segments
                        if (startIndex != -1)
                        {
                            // CrLf
                            if (startSegment.Ending == LineEnding.Cr)
                            {
                                if (lastSegment.Ending == LineEnding.Lf && lastSegment.Length == 1)
                                {
                                    startSegment.Length++;

                                    startSegment.Ending = LineEnding.CrLf;

                                    segments[startIndex] = startSegment;

                                    segmentCount++;

                                    lineOffset--;
                                }
                            }
                            else
                            {
                                startSegment.Length += lastSegment.Length;

                                startSegment.Ending = lastSegment.Ending;

                                segments[startIndex] = startSegment;

                                segmentCount++;
                            }
                        }
                    }


                    segments.RemoveRange(startIndex + 1, segmentCount);

                    Adjust(lineOffset, positionOffset, startIndex + 1);
                }
            }
        }

        // Clear
        public void Clear()
        {
            this.segments.Clear();

            this.segments.Add((0, 0, 0, LineEnding.None));

            this.length = 0;
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