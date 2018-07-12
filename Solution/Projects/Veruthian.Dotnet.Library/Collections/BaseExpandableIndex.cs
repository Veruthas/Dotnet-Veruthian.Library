using System.Collections.Generic;

namespace Veruthian.Dotnet.Library.Collections
{
    public abstract class BaseExpandableIndex<T> : BaseMutableIndex<T>, IExpandableIndex<T>
    {
        public abstract void Add(T value);

        public abstract void AddRange(IEnumerable<T> values);


        public abstract void Insert(int index, T value);

        public abstract void InsertRange(int index, IEnumerable<T> values);


        public abstract bool Remove(T value);
        
        public abstract void RemoveBy(int index);

        public abstract void RemoveRange(int start, int count);


        public abstract void Clear();
    }
}