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


        // String
        public override string ToString() => this.ToListString();


        // Creation        
        protected static TVector Make(Number capacity, Number size)
            => Make(new T[capacity.ToCheckedInt()], size);

        protected static TVector Make(Number capacity)
            => Make(new T[capacity.ToCheckedInt()], capacity.ToCheckedInt());

        protected static TVector Make(T[] items)
            => Make(items, items.Length);

        protected static TVector Make(T[] items, Number size)
        {
            var vector = new TVector();

            vector.items = items;

            vector.Size = size;

            return vector;
        }


        public static TVector New() => new TVector();

        public static TVector New(Number size) => Make(size);

        public static TVector Of(T item) => Make(new T[] { item });

        public static TVector Of(Number multiple, T item) => Make(item.RepeatAsArray(multiple.ToCheckedSignedInt()));

        public static TVector From(params T[] items) => Make(items.Copy());

        public static TVector From(Number mutiple, params T[] items) => Make(items.Multiply(mutiple.ToCheckedSignedInt()));

        public static TVector Extract(IEnumerable<T> items) => Make(items.ToArray());

        public static TVector Extract(IEnumerable<T> items, Number amount) => Make(items.ToArray(amount.ToCheckedSignedInt()));
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