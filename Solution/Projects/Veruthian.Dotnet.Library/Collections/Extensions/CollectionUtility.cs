using System;
using System.Collections.Generic;
using System.IO;

namespace Veruthian.Dotnet.Library.Collections.Extensions
{
    public static class CollectionExtensions
    {
        // Array extensions
        public static T[] Resize<T>(this T[] array, int newSize)
        {
            T[] newArray = new T[newSize];

            Array.Copy(array, newArray, Math.Min(newSize, array.Length));

            return newArray;
        }

        // Repeat
        public static IEnumerable<T> RepeatAsEnumerable<T>(this T value, int times = 1)
        {
            for (int i = 0; i < times; i++)
                yield return value;
        }

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
    }    
}
