namespace Veruthian.Library.Collections
{
    public abstract class BaseMutableIndex<T> : BaseIndex<T>, IMutableIndex<T>
    {
        public new T this[int index]
        {
            get
            {
                VerifyIndex(index);

                return RawGet(index);
            }
            set
            {
                VerifyIndex(index);

                RawSet(index, value);
            }

        }

        public bool TrySet(int index, T value)
        {
            if (IsValidIndex(index))
            {
                RawSet(index, value);

                return true;
            }

            return false;
        }

        protected abstract void RawSet(int verifiedIndex, T value);
    }
}