using System.Collections.Generic;

namespace Veruthian.Dotnet.Library.Data.Collections
{
    public abstract class ItemMutableLookupBase<TKey, TValue> : ItemLookupBase<TKey, TValue>, IMutableLookup<TKey, TValue>
    {
        public new TValue this[TKey key]
        {
            get => base[key];
            set
            {
                if (!TrySet(key, value))
                    throw new KeyNotFoundException();
            }
        }

        public abstract bool TrySet(TKey key, TValue value);
    }
}