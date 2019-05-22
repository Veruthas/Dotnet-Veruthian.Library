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
        public BaseVector() => items = new T[0];


        // TVector
        protected abstract TVector This { get; }

        // Items
        private T[] items;

        protected T[] Items
        {
            get => this.items;
            set => this.items = value ?? new T[0];
        }

        public static bool IsNullOrEmpty(TVector value) => value == null || value.Count.IsZero;


        public virtual Number Count => items.Length;


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

            ExceptionHelper.VerifyPositiveInBounds(index, offset.ToCheckedSignedInt(), 0, (int)Count, nameof(address), nameof(offset));
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
                var count = (int)Count;

                if (value == null)
                {
                    for (int i = 0; i < count; i++)
                    {
                        if (items[i] == null)
                            return i;
                    }
                }
                else
                {
                    for (int i = 0; i < count; i++)
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

                var count = (int)Count;

                for (int i = 0; i < count; i++)
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

                var count = (int)Count;

                for (int i = 0; i < count; i++)
                {
                    yield return (address, items[i]);

                    address = address.Next;
                }
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            var count = (int)Count;

            for (var i = 0; i < count; i++)
                yield return items[i];
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();


        // Extract
        public TVector Extract(A start)
        {
            VerifyAddress(start);

            var index = VerifiedAddressToIndex(start);

            var count = (int)Count;

            var newItems = items.Extracted(index, count - index);

            return Create(newItems);
        }

        public TVector Extract(A start, Number amount)
        {
            VerifyOffset(start, amount);

            var index = VerifiedAddressToIndex(start);

            var newItems = items.Extracted(index, (int)amount);

            return Create(newItems);
        }


        // String
        public override string ToString() => this.ToListString();


        // Creation
        protected static TVector Create(T[] items)
        {
            var vector = new TVector();

            vector.Initialize(items);

            return vector;
        }

        protected virtual void Initialize(T[] items) => this.Items = items;


        public static TVector New() => new TVector();

        public static TVector New(Number size) => Create(new T[size.ToCheckedInt()]);


        public static TVector Of(T item) => Create(new T[] { item });


        public static TVector From(params T[] items) => Create(items.Copy());

        public static TVector From(T[] items, int start) => Create(items.Extracted(start));

        public static TVector From(T[] items, int start, int amount) => Create(items.Extracted(start, amount));


        public static TVector Repeat(T item, Number mutiple) => Create(item.RepeatAsArray(mutiple.ToCheckedSignedInt()));

        public static TVector Repeat(T[] items, Number mutiple) => Create(ArrayExtensions.Repeated(items, mutiple.ToCheckedSignedInt()));

        public static TVector Repeat(TVector value, Number multiple) => Create(ArrayExtensions.Repeated(value.items, multiple.ToCheckedSignedInt()));

        public static TVector Repeat(IEnumerable<T> items, Number mutiple) => Create(ArrayExtensions.Repeated(items.ToArray(), mutiple.ToCheckedSignedInt()));


        public static TVector Extract(IEnumerable<T> items) => Create(items.ToArray());

        public static TVector Extract(IEnumerable<T> items, Number amount) => Create(items.ToArray(amount.ToCheckedSignedInt()));
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