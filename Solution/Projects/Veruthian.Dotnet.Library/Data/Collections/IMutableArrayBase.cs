using System;

namespace Veruthian.Dotnet.Library.Data.Collections
{
    public abstract class IMutableArrayBase<T> : ItemArrayBase<T>, IMutableLookup<int, T>
    {
        public new T this[int key]
        {
            get => base[key];
            set {
                if (!TrySet(key, value))
                    throw new IndexOutOfRangeException();
            }
        }

        public bool TrySet(int key, T value)
        {
            throw new System.NotImplementedException();
        }
    }
}