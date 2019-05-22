using System;
using System.Collections.Generic;
using System.Linq;
using Veruthian.Library.Utility;

namespace Veruthian.Library.Collections.Extensions
{
    public static class ArrayExtensions
    {
        // Resize
        public static T[] Resize<T>(this T[] array, int size)
        {
            ExceptionHelper.VerifyPositive(size, nameof(size));

            if (array == null || array.Length == 0 || size == 0)
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

        // Clear
        public static void Clear<T>(this T[] array)
        {
            ExceptionHelper.VerifyNotNull(array, nameof(array));

            Array.Clear(array, 0, array.Length);
        }

        public static void Clear<T>(this T[] array, int start)
        {
            ExceptionHelper.VerifyNotNull(array, nameof(array));

            ExceptionHelper.VerifyAtLeast(start, array.Length, nameof(start));

            Array.Clear(array, start, array.Length);
        }

        public static void Clear<T>(this T[] array, int start, int length)
        {
            ExceptionHelper.VerifyNotNull(array, nameof(array));

            ExceptionHelper.VerifyPositiveInBounds(start, length, 0, array.Length - 1, nameof(start), nameof(length));

            Array.Clear(array, start, length);
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


        // Extract
        public static T[] Extracted<T>(this T[] array, int start)
        {
            ExceptionHelper.VerifyAtMost(start, array.Length, nameof(start));

            var size = array.Length - start;

            var newArray = new T[size];

            array.CopyTo(newArray, start);

            return newArray;
        }

        public static T[] Extracted<T>(this T[] array, int start, int amount)
        {
            ExceptionHelper.VerifyPositiveInBounds(start, amount, 0, array.Length, nameof(start), nameof(amount));

            var newArray = new T[amount];

            array.CopyTo(newArray, start, 0, amount);

            return newArray;
        }


        // Move
        public static void Move<T>(this T[] array, int index, int amount, int? size = null)
        {
            if (size == 0)
                return;
            else if (size != null)
                ExceptionHelper.VerifyBetween(size.Value, 0, array.Length, nameof(size));
            else
                size = array.Length;

            ExceptionHelper.VerifyInBounds(index, amount, 0, size.Value, nameof(index), nameof(amount));

            Array.Copy(array, index, array, index + amount, size.Value - index);
        }


        // Insert Space
        public static T[] AppendedSpace<T>(this T[] array, int amount)
        {
            ExceptionHelper.VerifyPositive(amount, nameof(amount));

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

        public static T[] PrependedSpace<T>(this T[] array, int amount)
        {
            ExceptionHelper.VerifyPositive(amount, nameof(amount));

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

        public static T[] InsertedSpace<T>(this T[] array, int index, int amount)
        {
            if (index == 0)
            {
                return PrependedSpace(array, amount);
            }
            else if (index == array.Length)
            {
                return AppendedSpace(array, amount);
            }
            else
            {
                ExceptionHelper.VerifyIndexInBounds(index, 0, array.Length);

                int size = array.Length + amount;

                var newArray = new T[size];

                Array.Copy(array, 0, newArray, 0, index);

                Array.Copy(array, index, newArray, index + amount, array.Length - index);

                return newArray;
            }
        }


        // Insert Item
        public static T[] Appended<T>(this T[] array, T item)
        {
            var newArray = array.AppendedSpace(1);

            newArray[array.Length] = item;

            return newArray;
        }

        public static T[] Prepended<T>(this T[] array, T item)
        {
            var newArray = array.PrependedSpace(1);

            newArray[0] = item;

            return newArray;
        }

        public static T[] Inserted<T>(this T[] array, int index, T item)
        {
            var newArray = array.InsertedSpace(index, 1);

            newArray[index] = item;

            return newArray;
        }


        // Insert Array
        public static T[] AppendedArray<T>(this T[] array, T[] items)
        {
            if (items == null)
            {
                return array.Copy();
            }
            else
            {
                var newArray = array.AppendedSpace(items.Length);

                newArray.CopyFrom(items, 0, array.Length, items.Length);

                return newArray;
            }
        }

        public static T[] PrependedArray<T>(this T[] array, T[] items)
        {
            if (items == null)
            {
                return array.Copy();
            }
            else
            {
                var newArray = array.PrependedSpace(items.Length);

                newArray.CopyFrom(items, 0, 0, items.Length);

                return newArray;
            }
        }

        public static T[] InsertedArray<T>(this T[] array, int index, T[] items)
        {
            if (items == null)
            {
                return array.Copy();
            }
            if (index == 0)
            {
                return array.PrependedArray(items);
            }
            else if (index == array?.Length)
            {
                return array.AppendedArray(items);
            }
            else
            {
                var newArray = array.InsertedSpace(index, items.Length);

                newArray.CopyFrom(items, 0, index, items.Length);

                return newArray;
            }
        }


        // Insert Enumerable
        public static T[] AppendedEnumerable<T>(this T[] array, IEnumerable<T> items)
        {
            return array.AppendedArray(items.ToArray());
        }

        public static T[] PrependedEnumerable<T>(this T[] array, IEnumerable<T> items)
        {
            return array.PrependedArray(items.ToArray());
        }

        public static T[] InsertedEnumerable<T>(this T[] array, int index, IEnumerable<T> items)
        {
            return array.InsertedArray(index, items.ToArray());
        }


        public static T[] AppendedEnumerable<T>(this T[] array, IEnumerable<T> items, int amount)
        {
            ExceptionHelper.VerifyPositive(amount, nameof(amount));

            var newArray = array.AppendedSpace(amount);

            var i = 0;

            foreach (var item in items)
            {
                if (i >= amount)
                    break;

                newArray[array.Length + i++] = item;
            }

            return newArray;
        }

        public static T[] PrependedEnumerable<T>(this T[] array, IEnumerable<T> items, int amount)
        {
            ExceptionHelper.VerifyPositive(amount, nameof(amount));

            var newArray = array.PrependedSpace(amount);

            var i = 0;

            foreach (var item in items)
            {
                if (i >= amount)
                    break;

                newArray[i++] = item;
            }

            return newArray;
        }

        public static T[] InsertedEnumerable<T>(this T[] array, int index, IEnumerable<T> items, int amount)
        {
            ExceptionHelper.VerifyPositive(amount, nameof(amount));

            var newArray = array.InsertedSpace(index, amount);

            var i = 0;

            foreach (var item in items)
            {
                if (i >= amount)
                    break;

                newArray[index + i++] = item;
            }

            return newArray;
        }



        // Insert Container
        public static T[] AppendedContainer<T>(this T[] array, IContainer<T> items)
        {
            if (items == null)
            {
                return array.Copy();
            }
            else
            {
                var newArray = array.AppendedSpace(items.Count.ToCheckedSignedInt());

                int index = 0;

                foreach (var item in items)
                    newArray[index++] = item;

                return newArray;
            }
        }

        public static T[] PrependedContainer<T>(this T[] array, IContainer<T> items)
        {
            if (items == null)
            {
                return array.Copy();
            }
            else
            {
                var newArray = array.AppendedSpace(items.Count.ToCheckedSignedInt());

                int index = array.Length;

                foreach (var item in items)
                    newArray[index++] = item;

                return newArray;
            }
        }

        public static T[] InsertedContainer<T>(this T[] array, int index, IContainer<T> items)
        {
            if (items == null)
            {
                return array.Copy();
            }
            else if (index == 0)
            {
                return array.PrependedContainer(items);
            }
            else if (index == array?.Length)
            {
                return array.AppendedContainer(items);
            }
            else
            {
                var newArray = array.InsertedSpace(index, items.Count.ToCheckedSignedInt());

                foreach (var item in items)
                    newArray[index++] = item;

                return newArray;
            }
        }


        // Remove
        public static T[] Removed<T>(this T[] array, int index, int count = 1)
        {
            ExceptionHelper.VerifyBetween(index, 0, array.Length - 1, nameof(index));

            ExceptionHelper.VerifyPositive(count, nameof(count));

            ExceptionHelper.VerifyBetween(index + count, 0, array.Length, nameof(count));


            var newsize = array.Length - count;

            var newarray = new T[newsize];

            var sourceIndex = index + count;

            var amount = array.Length - sourceIndex;

            newarray.CopyFrom(array, 0, 0, index);

            newarray.CopyFrom(array, sourceIndex, index, amount);

            return newarray;
        }


        // Combine
        public static T[] Combined<T>(params T[][] arrays) => Combined((IEnumerable<T[]>)arrays);

        public static T[] Combined<T>(IEnumerable<T[]> arrays) => Combined(arrays.GetEnumerator(), 0);

        private static T[] Combined<T>(IEnumerator<T[]> arrays, int size)
        {
            if (arrays.MoveNext())
            {
                var array = arrays.Current;

                var newArray = Combined(arrays, size + array.Length);

                newArray.CopyFrom(array, 0, size, array.Length);

                return newArray;
            }
            else
            {
                return new T[size];
            }
        }


        // Repeat
        public static T[] Repeated<T>(this T[] array, int times)
        {
            ExceptionHelper.VerifyPositive(times, nameof(times));

            var newArray = new T[array.Length * times];

            for (int i = 0; i < times; i++)
                newArray.CopyFrom(array, 0, i * array.Length, array.Length);

            return newArray;
        }

        // Reverse
        public static void Reverse<T>(this T[] array)
        {
            ExceptionHelper.VerifyNotNull(array, nameof(array));

            for (int i = 0, j = array.Length - 1; i < array.Length / 2; i++, j--)
            {
                (array[i], array[j]) = (array[j], array[i]);
            }
        }

        public static T[] Reversed<T>(this T[] array)
        {
            ExceptionHelper.VerifyNotNull(array, nameof(array));

            var newArray = new T[array.Length];

            for (int i = 0, j = array.Length - 1; i < array.Length / 2; i++, j--)
            {
                (newArray[i], newArray[j]) = (array[j], array[i]);
            }

            return newArray;
        }
    }
}