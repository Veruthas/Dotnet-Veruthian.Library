using Veruthian.Library.Numeric;

namespace Veruthian.Library.Collections
{
    public abstract class BaseMutableVector<A, T, TVector> : BaseVector<A, T, TVector>, IMutableVector<A, T>
        where A : ISequential<A>
        where TVector : BaseMutableVector<A, T, TVector>, new()
    {
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


        protected virtual T RawSet(A verifiedAddress, T value) => this.Items[VerifiedAddressToIndex(verifiedAddress)] = value;
    }

    public abstract class BaseMutableVector<T, TVector> : BaseMutableVector<Number, T, TVector>, IMutableVector<T>
       where TVector : BaseMutableVector<T, TVector>, new()
    {
        public override Number Start => Number.Zero;

        protected override int VerifiedAddressToIndex(Number address) => (int)address;

        protected override Number OffsetStartAddress(Number offset) => offset;

        public override bool IsValidAddress(Number address) => address < Count;
    }
}