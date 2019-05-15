using System.Collections.Generic;

namespace Veruthian.Library.Collections
{
    public interface IResizableContainer<T> : IContainer<T>
    {
        void AddRange(IEnumerable<T> values);
        
        void Add(T value);

        bool Remove(T value);

        void Clear();
    }
}