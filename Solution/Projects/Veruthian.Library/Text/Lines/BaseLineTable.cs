using System.Collections;
using System.Collections.Generic;
using Veruthian.Library.Text.Encodings;
using Veruthian.Library.Text.Runes;

namespace Veruthian.Library.Text.Lines
{
    public abstract class BaseLineTable<I, S> : IEnumerable<(int Start, int Length, LineEnding ending)>
        where S : IEnumerable<I>
    {
        List<(int Start, int Length, LineEnding Ending)> lines;

        int length;

        LineEnding endingType;


        public BaseLineTable() : this(LineEnding.None) { }

        public BaseLineTable(LineEnding endingType)
        {
            this.lines = new List<(int Start, int Length, LineEnding Ending)>();

            this.lines.Add((0, 0, LineEnding.None));

            this.length = 0;

            this.endingType = endingType;
        }



        public int Count => lines.Count;

        public (int Start, int Length, LineEnding ending) GetLine(int lineNumber) => lines[lineNumber];

        public int GetLineNumber(int position)
        {
            var low = 0;

            var high = lines.Count - 1;

            while (low <= high)
            {
                var middle = (low + high) / 2;

                var line = lines[middle];

                if (position < line.Start)
                    high = middle - 1;
                else if (position >= line.Start + line.Length)
                    low = middle + 1;
                else
                    return middle;
            }

            return -1;
        }

        public TextLocation GetTextLocation(int position)
        {
            var lineNumber = GetLineNumber(position);

            var line = lines[lineNumber];

            return new TextLocation(position, lineNumber, position - line.Start);
        }



        public void Prepend(I value)
        {

        }

        public void Prepend(IEnumerable<I> value)
        {

        }

        public void Append(I value)
        {
            var index = lines.Count - 1;

            var line = lines[index];

            var utf32 = ConvertToUtf32(value);


            if ((line.Length == 0) && (utf32 == Utf32.Chars.Lf) && (index != 0))
            {
                var lastIndex = index - 1;

                var lastLine = lines[lastIndex];

                if (lastLine.Ending == LineEnding.Cr)
                {
                    lines[lastIndex] = (lastLine.Start, lastLine.Length + 1, LineEnding.CrLf);

                    lines[index] = (line.Start + 1, line.Length, line.Ending);

                    length++;

                    return;
                }
            }

            var ending = LineEnding.From(utf32);

            lines[index] = (line.Start, line.Length + 1, line.Ending);

            length++;

            if (ending.IsNewLine())
                lines.Add((length, 0, LineEnding.None));
        }

        public void Append(IEnumerable<I> values)
        {
            foreach (var value in values)
                Append(value);
        }


        public void Insert(int index, I value)
        {
            if (index == 0)
                Prepend(value);
            else if (index == length)
                Append(value);
            else
            {

            }
        }

        public void Insert(int index, IEnumerable<I> values)
        {
            if (index == 0)
                Prepend(values);
            else if (index == length)
                Append(values);
            else
            {

            }
        }

        public void Remove(int index, int amount)
        {

        }


        private void ResyncLines(int lineIndex, int offset)
        {
            for (int i = lineIndex; i < lines.Count; lineIndex++)
            {
                var line = lines[i];

                line = (line.Start + offset, line.Length, line.Ending);
            }
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
            if (lineNumber > 0 && lineNumber < lines.Count)
            {
                var line = lines[lineNumber];

                return ExtractLine(value, line.Start, line.Length - (includeEnd ? 0 : line.Ending.Size));
            }
            else
            {
                return default(S);
            }
        }

        public IEnumerable<S> ExtractLines(S value, bool includeEnd = true)
        {
            foreach (var line in lines)
                yield return ExtractLine(value, line.Start, line.Length - (includeEnd ? 0 : line.Ending.Size));
        }



        public IEnumerator<(int Start, int Length, LineEnding ending)> GetEnumerator() => this.lines.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();


        protected abstract uint ConvertToUtf32(I value);

        protected abstract int GetLength(S value);

        protected abstract S Slice(S value, int start, int length);
    }
}