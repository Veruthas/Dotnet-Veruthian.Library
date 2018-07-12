using System.Collections.Generic;

namespace Veruthian.Dotnet.Library.Collections
{
    public interface IExpandableIndex<T> : IMutableIndex<T>, IExpandableContainer<T>, IExpandableLookup<int, T>
    {
        void AddRange(IEnumerable<T> values);

        void InsertRange(int key, IEnumerable<T> values);
        
        void RemoveRange(int key, int count);
    }
}