using System.Collections.Generic;

namespace Veruthian.Library.Collections.Extensions
{
    public static class RepetitionExtensions
    {
        public static IEnumerable<T> Repeat<T>(this T value, int times = 1) => Enumerables.Repeat(value, times);

        public static T[] RepeatAsArray<T>(this T value, int times = 1)
        {
            var items = new T[times];

            for (int i = 0; i < times; i++)
                items[i] = value;

            return items;
        }

        public static List<T> RepeatAsList<T>(this T value, int times = 1)
        {
            var items = new List<T>(times);

            for (int i = 0; i < times; i++)
                items.Add(value);

            return items;
        }

        public static DataArray<T> RepeatAsDataArray<T>(this T value, int times = 1)
        {
            var items = DataArray<T>.New(times);

            for (int i = 0; i < times; i++)
                items[i] = value;

            return items;
        }

        public static DataList<T> RepeatAsDataList<T>(this T value, int times = 1)
        {
            var items = DataList<T>.NewWith(times);

            for (int i = 0; i < times; i++)
                items.Add(value);

            return items;
        }
    }
}