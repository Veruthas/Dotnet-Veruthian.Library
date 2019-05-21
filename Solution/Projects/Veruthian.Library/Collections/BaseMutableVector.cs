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


        protected virtual T RawSet(A verifiedAddress, T value) => items[VerifiedAddressToIndex(verifiedAddress)] = value;
    }
}