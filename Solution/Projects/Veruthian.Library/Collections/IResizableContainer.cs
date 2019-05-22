using System.Collections.Generic;

namespace Veruthian.Library.Collections
{
    public interface IResizableContainer<T> : IContainer<T>
    {
        void Add(T value);

        void Add(IEnumerable<T> values);
        
        bool Remove(T value);

        void Clear();
    }
}