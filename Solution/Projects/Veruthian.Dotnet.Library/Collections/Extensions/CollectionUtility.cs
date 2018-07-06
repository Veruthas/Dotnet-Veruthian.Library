using System;
using System.Collections.Generic;
using System.IO;

namespace Veruthian.Dotnet.Library.Collections.Extensions
{
    public static class CollectionExtensions
    {
        // Stream Enumerator
        public static IEnumerator<byte> GetEnumerator(this Stream stream)
        {
            while (true)
            {
                int value = stream.ReadByte();

                if (value != -1)
                    yield return (byte)value;
                else
                    yield break;
            }
        }    

        // NotifyingEnumerator
        public static NotifyingEnumerator<T> GetNotifyingEnumerator<T>(this IEnumerator<T> enumerator, EnumeratorMoveNext<T> onMoveNext = null)
        {
            var notifyer = new NotifyingEnumerator<T>(enumerator);

            notifyer.MovedNext += onMoveNext;

            return notifyer;
        }

        public static NotifyingEnumerator<T> GetNotifyingEnumerator<T>(this IEnumerable<T> enumerable, EnumeratorMoveNext<T> onMoveNext = null)
        {
            return GetNotifyingEnumerator(enumerable.GetEnumerator(), onMoveNext);
        }


        // Array extensions
        public static T[] ToArray<T>(this ILookup<int, T> items)
        {
            var array = new T[items.Count];

            for (int i = 0; i < items.Count; i++)
                array[i] = items[i];

            return array;
        }

        public static T[] Resize<T>(this T[] array, int newSize)
        {
            T[] newArray = new T[newSize];

            Array.Copy(array, newArray, Math.Min(newSize, array.Length));

            return newArray;
        }
  

  
        // Repeat
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

        public static IEnumerable<T> RepeatAsEnumerable<T>(this T value, int times = 1)
        {
            for (int i = 0; i < times; i++)
                yield return value;
        }
    }    
}
