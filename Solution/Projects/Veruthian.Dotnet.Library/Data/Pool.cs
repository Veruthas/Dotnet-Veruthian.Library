using System.Collections.Generic;

namespace Veruthian.Dotnet.Library.Text.Lexers
{
    public class Pool<TItem, TAttribute>
    {
        public struct PoolItem
        {
            public TItem Value { get; }

            public TAttribute Attribute { get; }


            public PoolItem(TItem value, TAttribute attribute)
            {
                this.Value = value;

                this.Attribute = attribute;
            }
        }

        Dictionary<TItem, PoolItem> pool;

        public Pool()
        {
            pool = new Dictionary<TItem, PoolItem>();
        }

        public Pool(IEqualityComparer<TItem> comparer)
        {
            pool = new Dictionary<TItem, PoolItem>(comparer);
        }

        public PoolItem Recall(TItem value, TAttribute attributeIfUndefined = default(TAttribute))
        {
            PoolItem result;

            if (!pool.TryGetValue(value, out result))
            {
                result = new PoolItem(value, attributeIfUndefined);

                pool.Add(value, result);
            }

            return result;
        }

        public PoolItem Set(TItem value, TAttribute attribute)
        {
            PoolItem result;
            
            if (pool.TryGetValue(value, out result))
            {
                result = new PoolItem(result.Value, attribute);
            }
            else
            {
                result = new PoolItem(value, attribute);
            }

            pool.Add(value, result);

            return result;
        }
    }
}