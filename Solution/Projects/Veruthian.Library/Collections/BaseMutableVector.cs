using Veruthian.Library.Numeric;

namespace Veruthian.Library.Collections
{
    public abstract class BaseMutableVector<T, TVector> : BaseVector<T, TVector>, IMutableVector<T>
        where TVector : BaseMutableVector<T, TVector>, new()
    {
        public new T this[Number address]
        {
            get
            {
                VerifyAddress(address);

                return RawGet(address);
            }
            set
            {
                VerifyAddress(address);

                RawSet(address, value);
            }

        }

        public bool TrySet(Number address, T value)
        {
            if (IsValidAddress(address))
            {
                RawSet(address, value);

                return true;
            }

            return false;
        }

        protected abstract void RawSet(Number verifiedAddress, T value);
    }
}