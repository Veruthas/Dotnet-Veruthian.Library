using System.Collections.Generic;

namespace Veruthian.Dotnet.Library.Data.Collections
{
    public class ItemSequentialLookup<TKey, TValue, TLookup, TSequence> : ILookup<TKey, TValue>
        where TLookup : ILookup<TKey, TValue>
        where TSequence : ILookup<int, TLookup>
    {
        TSequence sequence;

        public ItemSequentialLookup(TSequence sequence) => this.sequence = sequence;


        public TSequence Sequence => sequence;


        public TValue this[TKey key] => TryGet(key, out var value) ? value : throw new KeyNotFoundException($"{key?.ToString() ?? ""} is not define.");

        public bool IsDefaultable => false;

        public int Count
        {
            get
            {
                if (sequence != null)
                {
                    var keys = new HashSet<TKey>();

                    foreach(var lookup in sequence.Values)
                    {
                        foreach( var key in lookup.Keys)
                        {
                            if (!keys.Contains(key))
                                keys.Add(key);
                        }
                    }

                    return keys.Count;
                }

                return 0;
            }
        }

        public IEnumerable<TKey> Keys
        {
            get
            {
                if (sequence != null)
                {
                    var keys = new HashSet<TKey>();

                    foreach (var lookup in sequence.Values)
                    {
                        foreach (var key in lookup.Keys)
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
        }

        public IEnumerable<TValue> Values
        {
            get
            {
                if (sequence != null)
                {
                    var keys = new HashSet<TKey>();

                    foreach (var lookup in sequence.Values)
                    {
                        foreach (var pair in lookup.Pairs)
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
        }

        public IEnumerable<KeyValuePair<TKey, TValue>> Pairs
        {
            get
            {
                if (sequence != null)
                {
                    var keys = new HashSet<TKey>();

                    foreach (var lookup in sequence.Values)
                    {
                        foreach (var pair in lookup.Pairs)
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
        }

        public bool HasKey(TKey key)
        {
            if (sequence != null)
            {
                foreach (var lookup in sequence.Values)
                {
                    if (HasKey(key))
                        return true;
                }
            }

            return false;
        }

        public bool TryGet(TKey key, out TValue value)
        {
            if (sequence != null)
            {
                foreach (var lookup in sequence.Values)
                {
                    if (TryGet(key, out value))
                        return true;
                }
            }

            value = default(TValue);

            return false;
        }
    }
}