using System.Collections.Generic;
using Veruthian.Library.Numeric;

namespace Veruthian.Library.Collections
{
    public class BaseResizableVector<T, TVector> : BaseMutableVector<T, TVector>, IResizableVector<T>
        where TVector : BaseResizableVector<T, TVector>, new()
    {
        private void Resize()
        {

        }

        public void Add(T value)
        {
            if (size == Capacity)
                Resize();

            items[(int)size] = value;

            size++;
        }

        public void AddRange(IEnumerable<T> values)
        {
            throw new System.NotImplementedException();
        }

        public void Insert(Number address, T value)
        {
            throw new System.NotImplementedException();
        }

        public void InsertRange(Number address, IEnumerable<T> values)
        {
            throw new System.NotImplementedException();
        }

        public bool Remove(T value)
        {
            throw new System.NotImplementedException();
        }

        public void RemoveBy(Number address)
        {
            throw new System.NotImplementedException();
        }

        public void RemoveRange(Number address, Number count)
        {
            throw new System.NotImplementedException();
        }

        public void Clear()
        {
            throw new System.NotImplementedException();
        }
    }
}