using System.Collections.Generic;
using System.Linq;
using Veruthian.Library.Collections.Extensions;
using Veruthian.Library.Numeric;

namespace Veruthian.Library.Collections
{
    public abstract class BaseResizableVector<A, T, TVector> : BaseMutableVector<A, T, TVector>, IResizableVector<A, T>
        where A : ISequential<A>
        where TVector : BaseResizableVector<A, T, TVector>, new()
    {
        private void Resize(int requestedSpace)
        {
            int newSize = (int)size + requestedSpace;

            int newCapacity = 0x4;

            while (newCapacity < newSize)
            {
                newCapacity <<= 1;
            }

            this.items = items.Resize(newCapacity);
        }

        public void Add(T value)
        {
            if (this.size == Capacity)
                Resize(1);

            this.items[(int)size] = value;

            this.size++;
        }

        public void AddRange(IEnumerable<T> values)
        {
            var items = values.ToArray();

            if (!HasCapacity(items.Length))
                Resize(items.Length);

            items.CopyTo(this.items, (int)size);

            this.size += items.Length;
        }

        public void Insert(A address, T value)
        {
            if (this.size == Capacity)
                Resize(1);

            VerifyAddress(address);

            var index = VerifiedAddressToIndex(address);

            this.items.Move(index, 1, this.size);

            this.items[index] = value;

            this.size++;
        }

        public void InsertRange(A address, IEnumerable<T> values)
        {
            var items = values.ToArray();

            if (!HasCapacity(items.Length))
                Resize(items.Length);

            VerifyAddress(address);

            var index = VerifiedAddressToIndex(address);

            this.items.Move(index, items.Length, size);

            items.CopyTo(this.items, 0, index, items.Length);

            this.size += items.Length;
        }

        public bool Remove(T value)
        {
            var index = IndexOf(value);

            if (index == -1)
            {
                this.items.Move(index, -1, (int)size);

                this.size--;

                return false;
            }
            else
            {
                return true;
            }
        }

        public void RemoveBy(A address)
        {
            VerifyAddress(address);

            var index = VerifiedAddressToIndex(address);

            this.items.Move(index, -1, (int)this.size);

            this.size--;
        }

        public void RemoveRange(A address, Number amount)
        {
            VerifyOffset(address, amount);

            var index = VerifiedAddressToIndex(address);

            var count = (int)amount;

            this.items.Move(index, -count, (int)this.size);

            this.size -= count;
        }

        public void Clear()
        {
            this.items.Clear();

            this.size = 0;
        }
    }
}