using System;
using System.Collections.Generic;

namespace Soedeum.Dotnet.Library.Collections
{
    public class Table<TKey, TValue> : ITable<TKey, TValue>
    {
        Dictionary<TKey, TValue> items = new Dictionary<TKey, TValue>();


        public virtual IEnumerable<TKey> Keys => items.Keys;

        public virtual IEnumerable<TValue> Values => items.Values;

        public virtual IEnumerable<KeyValuePair<TKey, TValue>> Items => items;


        public virtual int Count => items.Count;

        public virtual bool IsEmpty => Count == 0;

        public virtual bool Define(TKey key, TValue value, bool overwrite = false)
        {
            if (overwrite || !IsDefined(key))
            {
                items[key] = value;
                return true;
            }
            else
            {
                return false;
            }
        }

        public virtual bool UnDefine(TKey key) => items.Remove(key);

        public virtual void Clear() => items.Clear();


        public virtual bool IsDefined(TKey key) => items.ContainsKey(key);


        public virtual bool TryResolve(TKey key, out TValue result)
        {
            return items.TryGetValue(key, out result);
        }


        public virtual TValue Resolve(TKey key) => Resolve(this, key);

        public virtual bool TryResolve<TCast>(TKey key, out TCast result)
            where TCast : TValue
            => TryResolve(key, out result);


        public static TValue Resolve(ITable<TKey, TValue> table, TKey key)
        {
            bool success = table.TryResolve(key, out TValue result);

            if (!success)
                throw new KeyNotFoundException();

            return result;
        }

        public static bool TryResolve<TCast>(ITable<TKey, TValue> table, TKey key, out TCast result)
            where TCast : TValue
        {
            bool success = table.TryResolve(key, out TValue baseResult);

            if (success && baseResult is TCast)
            {
                result = (TCast)baseResult;
                return true;
            }
            else
            {
                result = default(TCast);
                return false;
            }
        }


        public static bool NestedTryResolve(ITable<TKey, TValue> table, ITable<TKey, TValue> parent, TKey key, out TValue value)
        {
            bool result = table.TryResolve(key, out value);

            if (!result)
                result = parent.TryResolve(key, out value);

            return result;
        }

        public static bool MultipleNestedTryResolve(ITable<TKey, TValue> table, ITable<TKey, TValue>[] parents, TKey key, out TValue value)
        {
            bool result = table.TryResolve(key, out value);

            if (!result)
            {
                foreach (var parent in parents)
                {
                    result = parent.TryResolve(key, out value);

                    if (result)
                        break;
                }
            }

            return result;
        }
    }
}