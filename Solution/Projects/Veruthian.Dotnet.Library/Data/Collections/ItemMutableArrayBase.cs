using System;

namespace Veruthian.Dotnet.Library.Data.Collections
{
    public abstract class ItemMutableArrayBase<T> : ItemArrayBase<T>, IMutableLookup<int, T>
    {
        public new T this[int key]
        {
            get => base[key];
            set {
                if (!TrySet(key, value))
                    throw new IndexOutOfRangeException();
            }
        }

        public abstract bool TrySet(int index, T value);
    }
}