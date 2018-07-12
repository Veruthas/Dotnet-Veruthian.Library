using System;

namespace Veruthian.Dotnet.Library.Collections
{
    public abstract class BaseMutableIndex<T> : BaseIndex<T>, IMutableIndex<T>
    {
        public new T this[int index]
        {
            get => base[index];
            set
            {
                if (!TrySet(index, value))
                    throw new IndexOutOfRangeException();
            }
        }

        public bool TrySet(int index, T value)
        {
            if (IsValidIndex(index))
            {
                RawSet(index, value);
                return true;
            }
            else
            {
                return false;
            }
        }

        protected abstract void RawSet(int adjustedValidIndex, T value);
    }
}