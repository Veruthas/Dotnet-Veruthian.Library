using System.Collections.Generic;

namespace Veruthian.Dotnet.Library.Data.Collections
{
    public class ItemNestedLookup<TKey, TValue, TUpper, TLower> : IResizeableLookup<TKey, TValue>
        where TUpper : ILookup<TKey, TValue>
        where TLower : IResizeableLookup<TKey, TValue>
    {

        TUpper upper;

        TLower lower;


        public ItemNestedLookup(TUpper upper, TLower lower)
        {
            this.upper = upper;

            this.lower = lower;
        }

        TUpper UpperLookup => upper;

        TLower LowerLookup => lower;



        public TValue this[TKey key]
        {
            get => TryGet(key, out var value) ? value : throw new KeyNotFoundException($"{key?.ToString() ?? ""} is not define.");
            set
            {
                if (HasKey(key))
                    lower[key] = value;
                else
                    throw new KeyNotFoundException($"{key?.ToString() ?? ""} is not defined."); ;
            }

        }

        TValue ILookup<TKey, TValue>.this[TKey key] => this[key];


        public int Count
        {
            get
            {
                var keys = new HashSet<TKey>();

                foreach (var key in lower.Keys)
                {
                    if (!keys.Contains(key))
                        keys.Add(key);
                }

                if (upper != null)
                {
                    foreach (var key in upper.Keys)
                    {
                        if (!keys.Contains(key))
                            keys.Add(key);
                    }
                }

                return keys.Count;
            }
        }


        public IEnumerable<TKey> Keys
        {
            get
            {
                var keys = new HashSet<TKey>();

                foreach (var key in lower.Keys)
                {
                    if (!keys.Contains(key))
                    {
                        keys.Add(key);

                        yield return key;
                    }
                }

                if (upper != null)
                {
                    foreach (var key in upper.Keys)
                    {
                        if (!keys.Contains(key))
                        {
                            keys.Add(key);

                            yield return key;
                        }
                    }
                }
            }
        }

        public IEnumerable<TValue> Values
        {
            get
            {
                var keys = new HashSet<TKey>();

                foreach (var pair in lower.Pairs)
                {
                    if (!keys.Contains(pair.Key))
                    {
                        keys.Add(pair.Key);

                        yield return pair.Value;
                    }
                }

                if (upper != null)
                {
                    foreach (var pair in upper.Pairs)
                    {
                        if (!keys.Contains(pair.Key))
                        {
                            keys.Add(pair.Key);

                            yield return pair.Value;
                        }
                    }
                }
            }
        }

        public IEnumerable<KeyValuePair<TKey, TValue>> Pairs
        {
            get
            {
                var keys = new HashSet<TKey>();

                foreach (var pair in lower.Pairs)
                {
                    if (!keys.Contains(pair.Key))
                    {
                        keys.Add(pair.Key);

                        yield return pair;
                    }
                }

                if (upper != null)
                {
                    foreach (var pair in upper.Pairs)
                    {
                        if (!keys.Contains(pair.Key))
                        {
                            keys.Add(pair.Key);

                            yield return pair;
                        }
                    }
                }
            }
        }


        public void Clear() => lower.Clear();

        public bool HasKey(TKey key) => lower.HasKey(key) || (upper != null && upper.HasKey(key));

        public void Insert(TKey key, TValue value) => lower.Insert(key, value);

        public void Remove(TKey key) => lower.Remove(key);

        public bool TryGet(TKey key, out TValue value)
        {
            return (lower.TryGet(key, out value)) ||
                   (upper != null && upper.TryGet(key, out value));
        }
    }
}