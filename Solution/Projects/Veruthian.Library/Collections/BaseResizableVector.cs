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
            int newSize = (int)this.Size + requestedSpace;

            int newCapacity = 0x4;

            while (newCapacity < newSize)
            {
                newCapacity <<= 1;
            }

            this.Items = (this.Items.Resize(newCapacity));
        }

        public void Add(T value)
        {
            if (this.Size == Capacity)
                Resize(1);

            this.Items[(int)this.Size] = value;

            this.Size++;
        }

        public void AddRange(IEnumerable<T> values)
        {
            var items = values.ToArray();

            if (!HasCapacity(items.Length))
                Resize(items.Length);

            items.CopyTo(this.Items, (int)this.Size);

            this.Size += items.Length;
        }

        public void Insert(A address, T value)
        {
            if (this.Size == Capacity)
                Resize(1);

            VerifyAddress(address);

            var index = VerifiedAddressToIndex(address);

            this.Items.Move(index, 1, (int)this.Size);

            this.Items[index] = value;

            this.Size++;
        }

        public void InsertRange(A address, IEnumerable<T> values)
        {
            var items = values.ToArray();

            if (!HasCapacity(items.Length))
                Resize(items.Length);

            VerifyAddress(address);

            var index = VerifiedAddressToIndex(address);

            this.Items.Move(index, items.Length, (int)this.Size);

            items.CopyTo(this.Items, 0, index, items.Length);

            this.Size += items.Length;
        }

        public bool Remove(T value)
        {
            var index = IndexOf(value);

            if (index == -1)
            {
                this.Items.Move(index, -1, (int)this.Size);

                this.Size--;

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

            this.Items.Move(index, -1, (int)this.Size);

            this.Size--;
        }

        public void RemoveRange(A address, Number amount)
        {
            VerifyOffset(address, amount);

            var index = VerifiedAddressToIndex(address);

            var count = (int)amount;

            this.Items.Move(index, -count, (int)this.Size);

            this.Size -= count;
        }

        public void Clear()
        {
            this.Items.Clear();

            this.Size = 0;
        }
    }
}