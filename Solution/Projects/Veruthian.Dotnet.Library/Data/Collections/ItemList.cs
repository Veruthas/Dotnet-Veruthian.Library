using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Veruthian.Dotnet.Library.Data.Collections
{
    public class ItemList<T> : IResizeableLookup<int, T>, IEnumerable<T>
    {
        List<T> items;

        bool defaultable;



        public T this[int index]
        {
            get => items[index];
            set => items[index] = value;
        }

        T ILookup<int, T>.this[int index] => items[index];


        public bool IsDefaultable => defaultable;
        
        public int Count => items.Count;

        

        public void Clear() => items.Clear();

        public void Insert(int index, T value) => items.Insert(index, value);

        public void Remove(int index) => items.RemoveAt(index);


        public bool HasKey(int index) => index >= 0 && index < Count;

        public bool TryGet(int index, out T value)
        {
            if (HasKey(index))
            {
                value = items[index];
                return true;
            }
            else
            {
                value = default(T);
                return false;
            }
        }


        public List<T> ToList() => new List<T>(items);


        public IEnumerator<T> GetEnumerator()
        {
            return ((IEnumerable<T>)items).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<T>)items).GetEnumerator();
        }

        IEnumerable<int> ILookup<int, T>.Keys
        {
            get
            {
                for (int i = 0; i < Count; i++)
                    yield return i;
            }
        }

        IEnumerable<T> ILookup<int, T>.Values => items;

        public IEnumerable<KeyValuePair<int, T>> Pairs
        {
            get
            {
                for (int i = 0; i < Count; i++)
                    yield return new KeyValuePair<int, T>(i, items[i]);

            }
        }
    }
}