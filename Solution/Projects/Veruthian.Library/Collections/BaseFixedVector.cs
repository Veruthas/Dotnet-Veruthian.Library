using System.Collections.Generic;
using Veruthian.Library.Collections.Extensions;
using Veruthian.Library.Numeric;

namespace Veruthian.Library.Collections
{
    public abstract class BaseFixedVector<A, T, TVector> : BaseVector<A, T, TVector>
        where A : ISequential<A>
        where TVector : BaseFixedVector<A, T, TVector>, new()
    {
        // Reversed
        public TVector Reversed()
            => Create(Items.Reversed());

        // Repeated
        public TVector Repeated(Number multiple)
            => Repeat(This, multiple);

        // Prepended
        public TVector Prepended(T value)
            => Create(Items.Prepended(value));

        public TVector Prepended(T[] values)
            => Create(Items.PrependedArray(values));

        public TVector Prepended(IEnumerable<T> values)
            => Create(Items.PrependedEnumerable(values));

        public TVector Prepended(IEnumerable<T> values, int amount)
            => Create(Items.PrependedEnumerable(values, amount));

        public TVector Prepended(IContainer<T> values)
            => Create(Items.PrependedContainer(values));

        public TVector Prepended(TVector value)
            => Create(Items.PrependedArray(value.Items));

        // Appended
        public TVector Appended(T value)
            => Create(Items.Appended(value));

        public TVector Appended(T[] values)
            => Create(Items.AppendedArray(values));

        public TVector Appended(IEnumerable<T> values)
            => Create(Items.AppendedEnumerable(values));

        public TVector Appended(IEnumerable<T> values, int amount)
            => Create(Items.AppendedEnumerable(values, amount));

        public TVector Appended(IContainer<T> values)
            => Create(Items.AppendedContainer(values));

        public TVector Appended(TVector value)
            => Create(Items.AppendedArray(value.Items));

        // Inserted
        public TVector Inserted(A address, T value)
        {
            VerifyAddress(address);

            var index = VerifiedAddressToIndex(address);

            return Create(Items.Inserted(index, value));
        }

        public TVector Inserted(A address, T[] values)
        {
            VerifyAddress(address);

            var index = VerifiedAddressToIndex(address);

            return Create(Items.InsertedArray(index, values));
        }

        public TVector Inserted(A address, IEnumerable<T> values)
        {
            VerifyAddress(address);

            var index = VerifiedAddressToIndex(address);

            return Create(Items.InsertedEnumerable(index, values));
        }

        public TVector Inserted(A address, IEnumerable<T> values, int amount)
        {
            VerifyAddress(address);

            var index = VerifiedAddressToIndex(address);

            return Create(Items.InsertedEnumerable(index, values, amount));
        }

        public TVector Inserted(A address, IContainer<T> values)
        {
            VerifyAddress(address);

            var index = VerifiedAddressToIndex(address);

            return Create(Items.InsertedContainer(index, values));
        }

        public TVector Inserted(A address, TVector value)
        {
            VerifyAddress(address);

            var index = VerifiedAddressToIndex(address);

            return Create(Items.InsertedArray(index, value.Items));
        }

        // Removed
        public TVector Removed(A address)
        {
            VerifyAddress(address);

            var index = VerifiedAddressToIndex(address);

            return Create(Items.Removed(index));
        }

        public TVector Removed(A address, Number amount)
        {
            VerifyAddress(address);

            var index = VerifiedAddressToIndex(address);

            return Create(Items.Removed(index, amount.ToCheckedSignedInt()));
        }
    }

    public abstract class BaseFixedVector<T, TVector> : BaseVector<Number, T, TVector>
        where TVector : BaseFixedVector<T, TVector>, new()
    {
        public override Number Start => Number.Zero;

        protected override int VerifiedAddressToIndex(Number address) => (int)address;

        protected override Number OffsetStartAddress(Number offset) => offset;

        public override bool IsValidAddress(Number address) => address < Count;
    }
}