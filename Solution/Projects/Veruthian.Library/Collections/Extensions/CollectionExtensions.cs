using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Veruthian.Library.Collections.Extensions
{
    public static class CollectionExtensions
    {
        // Helpers

        private static void VerifyPositive(int value, string argument)
        {
            if (value < 0)
                throw new ArgumentException(argument);
        }

        private static void VerifyBounds(int index, int start, int end)
        {
            if (index < start || index >= end)
                throw new IndexOutOfRangeException();
        }        

        // Array extensions
        public static T[] Resize<T>(this T[] array, int size)
        {
            VerifyPositive(size, nameof(size));

            if (array == null || array.Length == 0)
            {
                return new T[size];
            }
            else
            {
                var newArray = new T[size];

                Array.Copy(array, newArray, Math.Min(size, array.Length));

                return newArray;
            }
        }



        public static T[] Append<T>(this T[] array, int amount)
        {
            VerifyPositive(amount, nameof(amount));

            if (array == null || array.Length == 0)
            {
                return new T[amount];
            }
            else
            {
                int size = array.Length + amount;

                var newArray = new T[size];

                Array.Copy(array, newArray, array.Length);

                return newArray;
            }
        }

        public static T[] Prepend<T>(this T[] array, int amount)
        {
            VerifyPositive(amount, nameof(amount));

            if (array == null || array.Length == 0)
            {
                return new T[amount];
            }
            else
            {                
                int size = array.Length + amount;

                var newArray = new T[size];

                Array.Copy(array, 0, newArray, amount, array.Length);

                return newArray;
            }
        }

        public static T[] Insert<T>(this T[] array, int index, int amount)
        {
            if (index == 0)
            {
                return Prepend(array, amount);
            }
            else if (index == array.Length)
            {
                return Append(array, amount);
            }
            else
            {
                VerifyBounds(index, 0, array.Length);

                int size = array.Length + amount;

                var newArray = new T[size];

                Array.Copy(array, 0, newArray, 0, index);

                Array.Copy(array, index, newArray, index + amount, array.Length - index);

                return newArray;
            }
        }

        public static T[] Copy<T>(this T[] array)
        {
            if (array == null)
            {
                return null;
            }            
            else
            {
                var newArray = new T[array.Length];

                Array.Copy(array, newArray, array.Length);

                return newArray;
            }
        }


        // Repeat
        public static IEnumerable<T> Repeat<T>(this T value, int times = 1)
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

        public static DataArray<T> RepeatAsDataArray<T>(this T value, int times = 1)
        {
            var items = new DataArray<T>(times);

            for (int i = 0; i < times; i++)
                items[i] = value;

            return items;
        }

        public static DataList<T> RepeatAsDataList<T>(this T value, int times = 1)
        {
            var items = new DataList<T>(times);

            for (int i = 0; i < times; i++)
                items.Add(value);

            return items;
        }

        // Strings
        public static string ToListString<T>(this IEnumerable<T> items, string start = "[", string end = "]", string separator = ", ")
        {
            var builder = new StringBuilder();

            builder.Append(start);

            bool started = false;

            foreach (var item in items)
            {
                if (started)
                    builder.Append(separator);
                else
                    started = true;

                builder.Append(item.ToString());
            }

            builder.Append(end);

            return builder.ToString();
        }

        public static string ToTableString<K, V>(this IEnumerable<(K, V)> items, string tableStart = "{", string tableEnd = "}",
                                                                                 string pairStart = "", string pairEnd = "", string pairSeparator = ",",
                                                                                 string keyStart = "[", string keyEnd = "] = ",
                                                                                 string valueStart = "'", string valueEnd = "'")
        {
            var builder = new StringBuilder();


            bool started = false;

            builder.Append(tableStart);

            foreach ((K key, V value) in items)
            {
                if (started)
                    builder.Append(pairSeparator);
                else
                    started = true;

                builder.Append(pairStart);

                // Key
                builder.Append(keyStart);
                builder.Append(key);
                builder.Append(keyEnd);

                // Value
                builder.Append(valueStart);
                builder.Append(value);
                builder.Append(valueEnd);

                builder.Append(pairEnd);
            }

            builder.Append(tableEnd);

            return builder.ToString();
        }
    }
}
