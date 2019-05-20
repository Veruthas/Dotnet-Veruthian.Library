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
        // Abstract Methods
        public abstract Number Count { get; }

        public abstract bool Contains(T value);

        protected abstract T RawGet(Number verifiedAddress);

        protected abstract void SetSize(Number size);

        protected abstract void SetData(T[] items);


        public Number Start => Number.Zero;

        public T this[Number address]
        {
            get
            {
                VerifyAddress(address);

                return RawGet(address);
            }
        }


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

        protected bool IsValidAddress(Number address) => (uint)address < Count;

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


        public static TVector New() => new TVector();

        public static TVector New(Number size)
        {
            var vector = new TVector();

            vector.SetSize(size);

            return vector;
        }

        private static TVector Make(T[] items)
        {
            var vector = new TVector();

            vector.SetData(items ?? new T[0]);

            return vector;
        }

        public static TVector Of(T item) => Make(new T[] { item });

        public static TVector From(params T[] items) => Make(items.Copy());

        public static TVector Extract(IEnumerable<T> items) => Make(items.ToArray());

        public static TVector Extract(IEnumerable<T> items, Number amount) => Make(items.ToArray(amount.ToCheckedSignedInt()));
    }
}