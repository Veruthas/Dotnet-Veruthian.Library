using System.Collections.Generic;
using Veruthian.Library.Text.Runes;

namespace Veruthian.Library.Text
{
    public class LineIndices
    {
        List<(int start, int length)> indices;

        TextLocation location;


        public LineIndices()
        {
            indices = new List<(int start, int length)>();

            indices.Add((0, 0));
        }


        private void MoveToNextUtf32(uint previous, uint current, uint next)
        {
        }

        public void MoveToNext(Rune previous, Rune current, Rune next) => MoveToNextUtf32(previous, current, next);

        public void MoveToNext(char previous, char current, char next) => MoveToNextUtf32(previous, current, next);


        public void MoveThrough(Rune previous, Rune current, IEnumerable<Rune> following)
        {
            foreach (var next in following)
            {
                MoveToNext(previous, current, next);

                previous = current;

                current = next;
            }
        }

        public void MoveThrough(char previous, char current, IEnumerable<char> following)
        {
            foreach (var next in following)
            {
                MoveToNext(previous, current, next);

                previous = current;

                current = next;
            }
        }
    }
}