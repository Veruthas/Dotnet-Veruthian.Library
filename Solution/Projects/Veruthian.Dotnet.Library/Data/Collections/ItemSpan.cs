using System.Collections.Generic;

namespace Veruthian.Dotnet.Library.Data.Collections
{
    public struct ItemSpan<T> : ILookup<int, T>
    {
        ILookup<int, T> lookup;

        int start;

        int count;

        T ILookup<int, T>.this[int index] => throw new System.NotImplementedException();

        int ILookup<int, T>.Count => throw new System.NotImplementedException();

        IEnumerable<int> ILookup<int, T>.Keys => throw new System.NotImplementedException();

        IEnumerable<T> ILookup<int, T>.Values => throw new System.NotImplementedException();

        IEnumerable<KeyValuePair<int, T>> ILookup<int, T>.Pairs => throw new System.NotImplementedException();

        bool ILookup<int, T>.HasKey(int index)
        {
            throw new System.NotImplementedException();
        }

        bool ILookup<int, T>.TryGet(int index, out T value)
        {
            throw new System.NotImplementedException();
        }
    }
}