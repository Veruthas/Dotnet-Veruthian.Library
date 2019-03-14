using System.Collections;
using System.Collections.Generic;
using Veruthian.Library.Text.Encodings;
using Veruthian.Library.Text.Runes;

namespace Veruthian.Library.Text.Lines
{
    // Due to Utf16 vs Utf32 differences, don't mix chars and Runes.
    public class LineIndexTable : IEnumerable<(int Start, int Length, LineEnding ending)>
    {
        List<(int Start, int Length, LineEnding Ending)> lines;


        public LineIndexTable()
        {
            lines = new List<(int Start, int Length, LineEnding Ending)>();

            lines.Add((0, -1, LineEnding.None));
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
                ending = LineEndings.GetNewLine(current, next);

                if (ending == LineEnding.Cr || ending == LineEnding.Lf)
                    lines.Add((line.Start + line.Length + 1, 0, LineEnding.None));
            }

            lines[lineIndex] = (line.Start, line.Length + 1, ending);
        }


        public void MoveToNext(Rune current, Rune next) => MoveToNextUtf32(current, next);

        public void MoveToNext(char current, char next) => MoveToNextUtf32(current, next);


        public void MoveThrough(Rune current, IEnumerable<Rune> following)
        {
            foreach (var next in following)
            {
                MoveToNext(current, next);

                current = next;
            }
        }

        public void MoveThrough(char current, IEnumerable<char> following)
        {
            foreach (var next in following)
            {
                MoveToNext(current, next);

                current = next;
            }
        }


        private RuneString ExtractLine(RuneString value, int start, int length)
        {
            if (start <= value.Length)
            {
                var end = start + length;

                if (end > value.Length)
                    end = value.Length - start;

                return value.Slice(start, length);
            }
            else
            {
                return null;
            }
        }

        public RuneString ExtractLine(RuneString value, int lineNumber, bool includeEnd = true)
        {
            if (lineNumber > 0 && lineNumber < lines.Count)
            {
                var line = lines[lineNumber];

                return ExtractLine(value, line.Start, line.Length - (includeEnd ? 0 : LineEndings.GetLineEndingSize(line.Ending)));
            }
            else
            {
                return null;
            }
        }

        public IEnumerable<RuneString> ExtractLines(RuneString value, bool includeEnd = true)
        {
            foreach (var line in lines)
                yield return ExtractLine(value, line.Start, line.Length - (includeEnd ? 0 : LineEndings.GetLineEndingSize(line.Ending)));
        }


        private string ExtractLine(string value, int start, int length)
        {
            if (start <= value.Length)
            {
                var end = start + length;

                if (end > value.Length)
                    end = value.Length - start;

                return value.Substring(start, length);
            }
            else
            {
                return null;
            }
        }

        public string ExtractLine(string value, int lineNumber, bool includeEnd = true)
        {
            if (lineNumber > 0 && lineNumber < lines.Count)
            {
                var line = lines[lineNumber];

                return ExtractLine(value, line.Start, line.Length - (includeEnd ? 0 : LineEndings.GetLineEndingSize(line.Ending)));
            }
            else
            {
                return null;
            }
        }

        public IEnumerable<string> ExtractLines(string value, bool includeEnd = true)
        {
            foreach (var line in lines)
                yield return ExtractLine(value, line.Start, line.Length - (includeEnd ? 0 : LineEndings.GetLineEndingSize(line.Ending)));
        }


        public IEnumerator<(int Start, int Length, LineEnding ending)> GetEnumerator() => this.lines.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}