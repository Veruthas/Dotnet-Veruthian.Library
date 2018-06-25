using System;

namespace Veruthian.Dotnet.Library.Data.Collections
{
    public abstract class MutableIndexBase<T> : IndexBase<T>, IMutableIndex<T>
    {
        protected MutableIndexBase(int startIndex = 0)
            : base(startIndex) { }

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
                RawSet(index + StartIndex, value);
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