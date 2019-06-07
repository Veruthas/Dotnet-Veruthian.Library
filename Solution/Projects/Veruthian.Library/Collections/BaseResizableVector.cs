using System.Collections.Generic;
using System.Linq;
using Veruthian.Library.Collections.Extensions;
using Veruthian.Library.Numeric;

namespace Veruthian.Library.Collections
{
    public abstract class BaseResizableVector<A, T, TVector> : BaseVector<A, T, TVector>, IMutableVector<A, T>, IResizableVector<A, T>
        where A : ISequential<A>
        where TVector : BaseResizableVector<A, T, TVector>, new()
    {
        private int size;

        protected Number Size
        {
            get => size;
            set => this.size = value.ToCheckedSignedInt();
        }

        protected virtual Number Capacity => Items.Length;

        protected bool HasCapacity(Number amount) => Count + amount <= Capacity;


        public override Number Count => Size;


        public new T this[A address]
        {
            get
            {
                return base[address];
            }
            set
            {
                VerifyAddress(address);

                RawSet(address, value);
            }

        }

        public bool TrySet(A address, T value)
        {
            if (IsValidAddress(address))
            {
                RawSet(address, value);

                return true;
            }

            return false;
        }

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


        public TVector Append(T value)
        {
            if (this.Size == Capacity)
                Resize(1);

            this.Items[(int)this.Size] = value;

            this.Size++;

            return This;
        }

        public TVector Append(IEnumerable<T> values)
        {
            var items = values is T[]? (T[])values : values.ToArray();

            if (!HasCapacity(items.Length))
                Resize(items.Length);

            items.CopyTo(this.Items, (int)this.Size);

            this.Size += items.Length;

            return This;
        }


        public TVector Prepend(T value)
        {
            Insert(Start, value);

            return This;
        }

        public TVector Prepend(IEnumerable<T> values)
        {
            Insert(Start, values);

            return This;
        }


        public TVector Insert(A address, T value)
        {
            if (this.Size == Capacity)
                Resize(1);

            VerifyAddress(address);

            var index = VerifiedAddressToIndex(address);

            this.Items.Move(index, 1, (int)this.Size);

            this.Items[index] = value;

            this.Size++;

            return This;
        }

        public TVector Insert(A address, IEnumerable<T> values)
        {
            var items = values is T[]? (T[])values : values.ToArray();

            if (!HasCapacity(items.Length))
                Resize(items.Length);

            VerifyAddress(address);

            var index = VerifiedAddressToIndex(address);

            this.Items.Move(index, items.Length, (int)this.Size);

            items.CopyTo(this.Items, 0, index, items.Length);

            this.Size += items.Length;

            return This;
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

        public TVector RemoveBy(A address)
        {
            VerifyAddress(address);

            var index = VerifiedAddressToIndex(address);

            this.Items.Move(index, -1, (int)this.Size);

            this.Size--;

            return This;
        }

        public TVector RemoveBy(A address, Number amount)
        {
            VerifyOffset(address, amount);

            var index = VerifiedAddressToIndex(address);

            var count = (int)amount;

            this.Items.Move(index, -count, (int)this.Size);

            this.Size -= count;

            return This;
        }


        public TVector Clear()
        {
            this.Items.Clear();

            this.Size = 0;

            return This;
        }
        

        void IResizableVector<A, T>.Append(T value)
            => Append(value);

        void IResizableVector<A, T>.Append(IEnumerable<T> values)
            => Append(values);


        void IResizableVector<A, T>.Prepend(T value)
            => Prepend(value);

        void IResizableVector<A, T>.Prepend(IEnumerable<T> values)
            => Prepend(values);


        void IResizableLookup<A, T>.Insert(A address, T value)
            => Insert(address, value);

        void IResizableVector<A, T>.Insert(A address, IEnumerable<T> values)
            => Insert(address, values);


        void IResizableLookup<A, T>.RemoveBy(A address)
            => RemoveBy(address);

        void IResizableVector<A, T>.RemoveBy(A address, Number amount)
            => RemoveBy(address, amount);


        void IResizableLookup<A, T>.Clear()
            => Clear();

        protected static TVector Create(Number capacity)
        {
            var vector = new TVector();

            vector.Initialize(new T[capacity.ToCheckedSignedInt()], Number.Zero);

            return vector;
        }

        protected virtual void Initialize(T[] items, Number size)
        {
            this.Items = items;

            this.Size = size;
        }

        protected override void Initialize(T[] items)
            => Initialize(items, items.Length);


        public static TVector Prepare(Number capacity)
            => Create(capacity);
    }

    public abstract class BaseResizableVector<T, TVector> : BaseResizableVector<Number, T, TVector>, IResizableVector<T>
        where TVector : BaseResizableVector<T, TVector>, new()
    {
        public override Number Start => Number.Zero;

        protected override int VerifiedAddressToIndex(Number address) => (int)address;

        protected override Number OffsetStartAddress(Number offset) => offset;

        public override bool IsValidAddress(Number address) => address < Count;
    }
}