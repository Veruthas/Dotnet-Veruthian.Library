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
    public abstract class BaseVector<T, TVector> : IVector<T>, IEnumerable<T>
        where TVector : BaseVector<T, TVector>, new()
    {
        protected T[] items;

        protected Number size;


        private static readonly T[] empty = new T[0];

        public BaseVector()
        {
            items = empty;

            size = Number.Zero;
        }


        protected virtual Number Capacity => items.Length;

        public virtual Number Count => size;


        protected virtual T RawGet(Number verifiedAddress) => items[(int)verifiedAddress];

        protected virtual T RawSet(Number verifiedAddress, T value) => items[(int)verifiedAddress] = value;


        public virtual bool Contains(T value)
        {
            {
                if (value == null)
                {
                    foreach (var item in items)
                    {
                        if (item == null)
                            return true;
                    }
                }
                else
                {
                    foreach (var item in items)
                    {
                        if (item.Equals(value))
                            return true;
                    }
                }

                return false;
            }
        }


        public T this[Number address]
        {
            get
            {
                VerifyAddress(address);

                return RawGet(address);
            }
        }


        public Number Start => Number.Zero;

        public bool TryGet(Number address, out T value)
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


        protected void VerifyAddress(Number address)
        {
            if (!IsValidAddress(address))
                throw new ArgumentOutOfRangeException(nameof(address));
        }

        protected bool IsValidAddress(Number address) => address.ToCheckedInt() < Count;

        bool ILookup<Number, T>.HasAddress(Number address) => IsValidAddress(address);

        IEnumerable<Number> ILookup<Number, T>.Addresses => Enumerables.GetRange(Number.Zero, Count - 1);

        public IEnumerable<(Number Address, T Value)> Pairs
        {
            get
            {
                for (var i = Number.Zero; i < Count; i++)
                    yield return (i, RawGet(i));
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (var i = Number.Zero; i < Count; i++)
                yield return RawGet(i);
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();



        public override string ToString() => this.ToListString();


        protected virtual void SetSize(Number size, Number capacity)
        {
            var vector = new TVector();

            vector.size = size;

            vector.items = new T[capacity.ToCheckedInt()];
        }

        protected virtual void SetData(T[] items)
        {
            var vector = new TVector();

            vector.SetData(items ?? new T[0]);
        }


        protected static TVector Make(Number size, Number capacity)
        {
            var vector = new TVector();

            vector.SetSize(size, capacity);

            return vector;
        }

        protected static TVector Make(Number size)
        {
            var vector = new TVector();

            vector.SetSize(size, size);

            return vector;
        }

        protected static TVector Make(T[] items)
        {
            var vector = new TVector();

            vector.SetData(items);

            return vector;
        }



        public static TVector New() => new TVector();

        public static TVector New(Number size) => Make(size);

        public static TVector Of(T item) => Make(new T[] { item });

        public static TVector From(params T[] items) => Make(items.Copy());

        public static TVector Extract(IEnumerable<T> items) => Make(items.ToArray());

        public static TVector Extract(IEnumerable<T> items, Number amount) => Make(items.ToArray(amount.ToCheckedSignedInt()));
    }
}