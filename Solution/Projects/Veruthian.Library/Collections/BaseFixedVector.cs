using Veruthian.Library.Numeric;

namespace Veruthian.Library.Collections
{
    public abstract class BaseFixedVector<A, T, TVector> : BaseVector<A, T, TVector>
        where A : ISequential<A>
        where TVector : BaseFixedVector<A, T, TVector>, new()
    {

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