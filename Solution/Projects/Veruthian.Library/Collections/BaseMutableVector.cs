using Veruthian.Library.Numeric;

namespace Veruthian.Library.Collections
{
    public abstract class BaseMutableVector<T> : BaseVector<T>, IMutableVector<T>
    {
        public new T this[Number address]
        {
            get
            {
                VerifyIndex(address);

                return RawGet(address);
            }
            set
            {
                VerifyIndex(address);

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