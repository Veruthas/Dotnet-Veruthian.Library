using System.Collections;
using System.Collections.Generic;

namespace Veruthian.Dotnet.Library.Collections
{
    public class DataArray<T> : IMutableIndex<T>, IEnumerable<T>
    {
        T[] items;


        public T this[int key] { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        T ILookup<int, T>.this[int key] => throw new System.NotImplementedException();

        int IIndex<int, T>.Start => 0;


        public int Count => throw new System.NotImplementedException();


        bool IsValidIndex(int index) => (uint)index < Count;

        bool IContainer<T>.Contains(T value)
        {
            throw new System.NotImplementedException();
        }

        bool ILookup<int, T>.HasKey(int key)
        {
            throw new System.NotImplementedException();
        }

        bool ILookup<int, T>.TryGet(int key, out T value)
        {
            throw new System.NotImplementedException();
        }

        bool IMutableLookup<int, T>.TrySet(int key, T value)
        {
            throw new System.NotImplementedException();
        }


        IEnumerable<int> ILookup<int, T>.Keys => Enumerables.GetRange(0, Count);

        IEnumerable<T> IContainer<T>.Values
        {
            get
            {
                foreach (var item in items)
                    yield return item;
            }
        }

        public IEnumerable<(int, T)> Pairs
        {
            get
            {
                for(var i = 0; i < Count; i++)
                    yield return (i, items[i]);
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            foreach(var item in items)
                yield return item;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}