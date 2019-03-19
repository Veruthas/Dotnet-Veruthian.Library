using System;
using System.Collections.Generic;
using System.Linq;

namespace Veruthian.Library.Collections.Extensions
{
    public static class ArrayExtensions
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


        // Resize
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


        // Copy
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


        // Copy To
        public static void CopyTo<T>(this T[] source, T[] destination)
        {
            Array.Copy(source, destination, source.Length);
        }

        public static void CopyTo<T>(this T[] source, T[] destination, int amount)
        {
            Array.Copy(source, destination, amount);
        }

        public static void CopyTo<T>(this T[] source, T[] destination, int sourceIndex, int destinationIndex, int amount)
        {
            Array.Copy(source, sourceIndex, destination, destinationIndex, amount);
        }


        // Copy From
        public static void CopyFrom<T>(this T[] destination, T[] source)
        {
            Array.Copy(source, destination, source.Length);
        }

        public static void CopyFrom<T>(this T[] destination, T[] source, int amount)
        {
            Array.Copy(source, destination, amount);
        }

        public static void CopyFrom<T>(this T[] destination, T[] source, int sourceIndex, int destinationIndex, int amount)
        {
            Array.Copy(source, sourceIndex, destination, destinationIndex, amount);
        }


        // Insert Space
        public static T[] AppendSpace<T>(this T[] array, int amount)
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

        public static T[] PrependSpace<T>(this T[] array, int amount)
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

        public static T[] InsertSpace<T>(this T[] array, int index, int amount)
        {
            if (index == 0)
            {
                return PrependSpace(array, amount);
            }
            else if (index == array.Length)
            {
                return AppendSpace(array, amount);
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


        // Insert
        public static T[] Append<T>(this T[] array, T item)
        {
            var newArray = array.AppendSpace(1);

            newArray[array.Length] = item;

            return newArray;
        }

        public static T[] Prepend<T>(this T[] array, T item)
        {
            var newArray = array.PrependSpace(1);

            newArray[0] = item;

            return newArray;
        }

        public static T[] Insert<T>(this T[] array, int index, T item)
        {
            var newArray = array.InsertSpace(index, 1);

            newArray[index] = item;

            return newArray;
        }


        // Insert Array
        public static T[] AppendArray<T>(this T[] array, T[] items)
        {
            if (items == null)
            {
                return array.Copy();
            }
            else
            {
                var newArray = array.AppendSpace(items.Length);

                newArray.CopyFrom(items, 0, array.Length, items.Length);

                return newArray;
            }
        }

        public static T[] PrependArray<T>(this T[] array, T[] items)
        {
            if (items == null)
            {
                return array.Copy();
            }
            else
            {
                var newArray = array.PrependSpace(items.Length);

                newArray.CopyFrom(items, 0, 0, items.Length);

                return newArray;
            }
        }

        public static T[] InsertArray<T>(this T[] array, int index, T[] items)
        {
            if (items == null)
            {
                return array.Copy();
            }
            if (index == 0)
            {
                return array.PrependArray(items);
            }
            else if (index == array?.Length)
            {
                return array.AppendArray(items);
            }
            else
            {
                var newArray = array.InsertSpace(index, items.Length);

                newArray.CopyFrom(items, 0, index, items.Length);

                return newArray;
            }
        }


        // Insert Enumerable
        public static T[] AppendEnumerable<T>(this T[] array, IEnumerable<T> items)
        {
            return array.AppendArray(items.ToArray());
        }

        public static T[] PrependEnumerable<T>(this T[] array, IEnumerable<T> items)
        {
            return array.PrependArray(items.ToArray());
        }

        public static T[] InsertEnumerable<T>(this T[] array, int index, IEnumerable<T> items)
        {
            return array.InsertArray(index, items.ToArray());
        }

       
        // Insert Container
        public static T[] AppendContainer<T>(this T[] array, IContainer<T> items)
        {
            if (items == null)
            {
                return array.Copy();
            }
            else
            {
                var newArray = array.AppendSpace(items.Count);

                int index = 0;

                foreach (var item in items)
                    newArray[index++] = item;

                return newArray;
            }
        }

        public static T[] PrependContainer<T>(this T[] array, IContainer<T> items)
        {
            if (items == null)
            {
                return array.Copy();
            }
            else
            {
                var newArray = array.AppendSpace(items.Count);

                int index = array.Length;

                foreach (var item in items)
                    newArray[index++] = item;

                return newArray;
            }
        }

        public static T[] InsertContainer<T>(this T[] array, int index, IContainer<T> items)
        {
            if (items == null)
            {
                return array.Copy();
            }
            else if (index == 0)
            {
                return array.PrependContainer(items);
            }
            else if (index == array?.Length)
            {
                return array.AppendContainer(items);
            }
            else
            {
                var newArray = array.InsertSpace(index, items.Count);

                foreach (var item in items)
                    newArray[index++] = item;

                return newArray;
            }
        }
    }
}