using System.Collections;
using System.Collections.Generic;
using Veruthian.Library.Text.Encodings;
using Veruthian.Library.Text.Runes;

namespace Veruthian.Library.Text.Lines
{
    public abstract class BaseLineTable<U, S> : IEnumerable<(int Start, int Length, LineEnding ending)>
        where S : IEnumerable<U>
    {
        List<(int Start, int Length, LineEnding Ending)> lines;

        int length;


        public BaseLineTable()
        {
            this.lines = new List<(int Start, int Length, LineEnding Ending)>();

            this.lines.Add((0, 0, LineEnding.None));

            this.length = 0;
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


        private void MoveToNextUtf32(uint current, uint next)
        {
            int lineIndex = lines.Count - 1;

            var line = lines[lineIndex];

            var ending = line.Ending;


            if (ending == LineEnding.CrLf)
            {
                lines.Add((line.Start + line.Length + 1, 0, LineEnding.None));
            }
            else
            {
                ending = LineEnding.FromValues(current, next);

                if (ending == LineEnding.Cr || ending == LineEnding.Lf)
                    lines.Add((line.Start + line.Length + 1, 0, LineEnding.None));
            }

            lines[lineIndex] = (line.Start, line.Length + 1, ending);
        }


        public void MoveToNext(U current, U next) => MoveToNextUtf32(ConvertToUtf32(current), ConvertToUtf32(next));


        public void MoveThrough(U current, IEnumerable<U> following)
        {
            foreach (var next in following)
            {
                MoveToNext(current, next);

                current = next;
            }
        }


        private int LastIndex => lines.Count - 1;


        public void Prepend(U value)
        {

        }

        public void Prepend(IEnumerable<U> value)
        {

        }

        public void Append(U value)
        {
            
        }

        public void Append(IEnumerable<U> values)
        {

        }


        public void Insert(int index, U value)
        {

        }

        public void Insert(int index, IEnumerable<U> value)
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


        protected abstract uint ConvertToUtf32(U value);

        protected abstract int GetLength(S value);

        protected abstract S Slice(S value, int start, int length);
    }
}