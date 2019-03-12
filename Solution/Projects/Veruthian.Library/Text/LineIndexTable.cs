using System.Collections.Generic;
using Veruthian.Library.Text.Encodings;
using Veruthian.Library.Text.Runes;

namespace Veruthian.Library.Text
{
    // Due to Utf16 vs Utf32 differences, don't mix chars and Runes.
    public class LineIndexTable
    {
        List<(int Start, int Length, LineEnding Ending)> table;


        public LineIndexTable()
        {
            table = new List<(int Start, int Length, LineEnding Ending)>();

            table.Add((0, -1, LineEnding.None));
        }


        private void MoveToNextUtf32(uint current, uint next)
        {
            int lineIndex = table.Count - 1;

            var line = table[lineIndex];

            var ending = line.Ending;


            if (ending == LineEnding.CrLf)
            {
                table.Add((line.Start + line.Length + 1, 0, LineEnding.None));
            }
            else
            {
                ending = Utf32.GetNewLine(current, next);

                if (ending == LineEnding.Cr || ending == LineEnding.Lf )
                    table.Add((line.Start + line.Length + 1, 0, LineEnding.None));                
            }

            table[lineIndex] = (line.Start, line.Length + 1, ending);
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
            if (lineNumber > 0 && lineNumber < table.Count)
            {
                var line = table[lineNumber];

                return ExtractLine(value, line.Start, line.Length - (includeEnd ? 0 : Utf32.GetLineEndingSize(line.Ending)));
            }
            else
            {
                return null;
            }
        }

        public IEnumerable<RuneString> ExtractLines(RuneString value, bool includeEnd = true)
        {
            foreach (var line in table)
                yield return ExtractLine(value, line.Start, line.Length - (includeEnd ? 0 : Utf32.GetLineEndingSize(line.Ending)));
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
            if (lineNumber > 0 && lineNumber < table.Count)
            {
                var line = table[lineNumber];

                return ExtractLine(value, line.Start, line.Length - (includeEnd ? 0 : Utf32.GetLineEndingSize(line.Ending)));
            }
            else
            {
                return null;
            }
        }

        public IEnumerable<string> ExtractLines(string value, bool includeEnd = true)
        {
            foreach (var line in table)
                yield return ExtractLine(value, line.Start, line.Length - (includeEnd ? 0 : Utf32.GetLineEndingSize(line.Ending)));
        }
    }
}