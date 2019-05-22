using System.Collections.Generic;
using Veruthian.Library.Numeric;

namespace Veruthian.Library.Collections
{
    public interface IResizableVector<A, T> : IResizableLookup<A, T>, IMutableVector<A, T>
        where A : ISequential<A>
    {        
        void Append(T value);

        void Append(IEnumerable<T> values);

        void Prepend(T value);

        void Prepend(IEnumerable<T> values);

        void Insert(A address, IEnumerable<T> values);

        bool Remove(T value);

        void RemoveBy(A address, Number amount);
    }

    public interface IResizableVector<V> : IResizableVector<Number, V>, IVector<V>
    {

    }
}