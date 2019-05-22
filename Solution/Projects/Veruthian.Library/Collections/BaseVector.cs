using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Veruthian.Library.Collections.Extensions;
using Veruthian.Library.Numeric;
using Veruthian.Library.Utility;

namespace Veruthian.Library.Collections
{
    public abstract class BaseVector<A, T, TVector> : IVector<A, T>
        where A : ISequential<A>
        where TVector : BaseVector<A, T, TVector>, new()
    {
        private T[] items;

        private int size;


        public BaseVector()
        {
            items = new T[0];

            size = 0;
        }


        protected Number Size
        {
            get => size;
            set
            {
                this.size = value.ToCheckedSignedInt();

                OnSizeSet();
            }
        }

        protected T[] Items
        {
            get => this.items;
            set
            {
                this.items = value ?? new T[0];

                OnItemsSet();
            }
        }


        protected virtual void OnSizeSet() { }

        protected virtual void OnItemsSet() { }

        public static bool IsNullOrEmpty(TVector value) => value == null || value.Size.IsZero;


        protected virtual Number Capacity => items.Length;

        protected bool HasCapacity(Number amount) => size + amount <= Capacity;

        public virtual Number Count => size;


        // Addresses
        protected abstract int VerifiedAddressToIndex(A address);

        protected abstract A OffsetStartAddress(Number offset);

        public abstract bool IsValidAddress(A address);

        public abstract A Start { get; }

        public A GetAddress(Number offset)
        {
            var address = OffsetStartAddress(offset);

            VerifyAddress(address);

            return address;
        }

        protected void VerifyAddress(A address)
        {
            if (!IsValidAddress(address))
                throw new ArgumentOutOfRangeException(nameof(address));
        }

        protected void VerifyOffset(A address, Number offset)
        {
            VerifyAddress(address);

            var index = VerifiedAddressToIndex(address);

            ExceptionHelper.VerifyPositiveInBounds(index, offset.ToCheckedSignedInt(), 0, size, nameof(address), nameof(offset));
        }



        // Access
        public T this[A address]
        {
            get
            {
                VerifyAddress(address);

                return RawGet(address);
            }
        }

        protected virtual T RawGet(A verifiedAddress) => items[VerifiedAddressToIndex(verifiedAddress)];

        protected virtual T RawSet(A verifiedAddress, T value) => this.Items[VerifiedAddressToIndex(verifiedAddress)] = value;
        
        public bool TryGet(A address, out T value)
        {
            if (IsValidAddress(address))
            {
                value = RawGet(address);

                return true;
            }
            else
            {
                value = default(T);

                return false;
            }
        }


        // Contains
        public virtual bool Contains(T value) => IndexOf(value) > -1;

        protected int IndexOf(T value)
        {
            {
                if (value == null)
                {
                    for (int i = 0; i < (int)size; i++)
                    {
                        if (items[i] == null)
                            return i;
                    }
                }
                else
                {
                    for (int i = 0; i < (int)size; i++)
                    {
                        if (items[i].Equals(value))
                            return i;
                    }
                }

                return -1;
            }
        }


        // Enumerables
        public IEnumerable<A> Addresses
        {
            get
            {
                var address = Start;

                for (int i = 0; i < size; i++)
                {
                    yield return address;

                    address = address.Next;
                }
            }
        }

        public IEnumerable<(A Address, T Value)> Pairs
        {
            get
            {
                var address = Start;

                for (int i = 0; i < size; i++)
                {
                    yield return (address, items[i]);

                    address = address.Next;
                }
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (var i = 0; i < size; i++)
                yield return items[i];
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();


        // Extract
        public TVector Extract(A start)
        {
            VerifyAddress(start);

            int index = VerifiedAddressToIndex(start);

            var newItems = items.Extract(index, size - index);

            return Create(newItems);
        }

        public TVector Extract(A start, Number amount)
        {
            VerifyOffset(start, amount);

            var index = VerifiedAddressToIndex(start);

            var newItems = items.Extract(index, (int)amount);

            return Create(newItems);
        }


        // String
        public override string ToString() => this.ToListString();


        // Creation        
        protected static TVector Create(Number capacity, Number size)
            => Create(new T[capacity.ToCheckedInt()], size);

        protected static TVector Create(Number capacity)
            => Create(new T[capacity.ToCheckedInt()], capacity.ToCheckedInt());

        protected static TVector Create(T[] items)
            => Create(items, items.Length);

        protected static TVector Create(T[] items, Number size)
        {
            var vector = new TVector();

            vector.items = items;

            vector.Size = size;

            return vector;
        }

        protected static TVector Create(T[] items, Number capacity, Number size)
        {
            var vector = new TVector();

            vector.items = items;

            vector.Size = size;

            return vector;
        }


        public static TVector New() => new TVector();

        public static TVector New(Number size) => Create(size);


        public static TVector Of(T item) => Create(new T[] { item });


        public static TVector From(params T[] items) => Create(items.Copy());

        public static TVector From(T[] items, int start) => Create(items.Extract(start));

        public static TVector From(T[] items, int start, int amount) => Create(items.Extract(start, amount));


        public static TVector Repeat(T item, Number mutiple) => Create(item.RepeatAsArray(mutiple.ToCheckedSignedInt()));

        public static TVector Repeat(T[] items, Number mutiple) => Create(ArrayExtensions.Repeat(items, mutiple.ToCheckedSignedInt()));

        public static TVector Repeat(TVector value, Number multiple) => Create(ArrayExtensions.Repeat(value.items, multiple.ToCheckedSignedInt()));

        public static TVector Repeat(IEnumerable<T> items, Number mutiple) => Create(ArrayExtensions.Repeat(items.ToArray(), mutiple.ToCheckedSignedInt()));


        public static TVector Extract(IEnumerable<T> items) => Create(items.ToArray());

        public static TVector Extract(IEnumerable<T> items, Number amount) => Create(items.ToArray(amount.ToCheckedSignedInt()));


        public static TVector Join(TVector left, TVector right)
        {
            if (IsNullOrEmpty(left))
                return Create(right.items);

            if (IsNullOrEmpty(right))
                return Create(left.items);

            var newSize = left.size + right.size;

            var newItems = new T[newSize];

            left.items.CopyTo(newItems, 0, 0, left.size);

            right.items.CopyTo(newItems, 0, left.size, right.size);

            return Create(newItems);
        }

        public static TVector Join(TVector left, T right)
        {
            if (IsNullOrEmpty(left))
                return Of(right);

            var newItems = left.items.Resize(left.size + 1);

            newItems[left.size] = right;

            return Create(newItems);
        }

        public static TVector Join(T left, TVector right)
        {
            if (IsNullOrEmpty(right))
                return Of(left);

            var newItems = new T[right.size + 1];

            newItems[0] = left;

            right.items.CopyTo(newItems, 0, 1, right.size);

            return Create(newItems);
        }
    }


    public abstract class BaseVector<T, TVector> : BaseVector<Number, T, TVector>, IVector<T>
        where TVector : BaseVector<T, TVector>, new()
    {
        public override Number Start => Number.Zero;

        protected override int VerifiedAddressToIndex(Number address) => (int)address;

        protected override Number OffsetStartAddress(Number offset) => offset;

        public override bool IsValidAddress(Number address) => address < Count;
    }
}