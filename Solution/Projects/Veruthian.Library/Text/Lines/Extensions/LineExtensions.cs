using System.Collections.Generic;

namespace Veruthian.Library.Text.Lines.Extensions
{
    public static class LineExtensions
    {
        public static IEnumerable<S> ExtractLines<S>(this IEnumerable<TextSegment> segments, S values, ExtractText<S> extractor)
        {
            foreach (var segment in segments)
            {
                var value = segment.Extract(values, extractor);

                yield return  value;
            }
        }

        public static IEnumerable<(TextSegment segment, S Value)> ExtractLineData<S>(this IEnumerable<TextSegment> segments, S values, ExtractText<S> extractor)
        {
            foreach (var segment in segments)
            {
                var value = segment.Extract(values, extractor);

                yield return (segment, value);
            }
        }
    }
}