using System;
using System.Collections;
using System.Collections.Generic;

namespace Veruthian.Dotnet.Library.Data.Collections
{
    public abstract class ItemArrayBase<T> : ILookup<int, T>, IEnumerable<T>
    {
        public T this[int index]
        {
            get => TryGet(index, out T value) ? value : throw new IndexOutOfRangeException();            
        }


        public abstract int Count { get; }


        public bool IsValidIndex(int index) => index >= 0 && index < Count;

        bool ILookup<int, T>.HasKey(int index) => IsValidIndex(index);

        public abstract bool Contains(T value);

        public abstract bool TryGet(int index, out T value);


        IEnumerable<int> ILookup<int, T>.Keys => GetKeys();

        IEnumerable<T> IContainer<T>.Values => GetValues();

        public IEnumerable<KeyValuePair<int, T>> Pairs => GetPairs();

        public IEnumerator<T> GetEnumerator() => GetValues().GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetValues().GetEnumerator();


        protected abstract IEnumerable<int> GetKeys();
        
        protected abstract IEnumerable<T> GetValues();

        protected abstract IEnumerable<KeyValuePair<int, T>> GetPairs();
    }
}