using System.Collections.Generic;
using System.Linq;
using Veruthian.Library.Collections.Extensions;
using Veruthian.Library.Numeric;

namespace Veruthian.Library.Collections
{
    public abstract class BaseResizableVector<T, TVector> : BaseMutableVector<T, TVector>, IResizableVector<T>
        where TVector : BaseResizableVector<T, TVector>, new()
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

        public void Insert(Number address, T value)
        {
            if (this.size == Capacity)
                Resize(1);

            var index = address.ToCheckedSignedInt();

            this.items.Move(address.ToCheckedSignedInt(), 1, this.size.ToSignedInt());

            this.items[index] = value;

            this.size++;
        }

        public void InsertRange(Number address, IEnumerable<T> values)
        {
            var items = values.ToArray();

            if (!HasCapacity(items.Length))
                Resize(items.Length);

            var index = address.ToCheckedSignedInt();

            this.items.Move(index, items.Length, size.ToSignedInt());

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

        public void RemoveBy(Number address)
        {
            VerifyAddress(address);

            this.items.Move((int)address, -1, (int)this.size);

            this.size--;
        }

        public void RemoveRange(Number address, Number amount)
        {
            VerifyRange(address, amount);

            this.items.Move((int)address, -(int)amount, (int)this.size);

            this.size -= amount;
        }

        public void Clear()
        {
            this.items.Clear();

            this.size = 0;            
        }
    }
}