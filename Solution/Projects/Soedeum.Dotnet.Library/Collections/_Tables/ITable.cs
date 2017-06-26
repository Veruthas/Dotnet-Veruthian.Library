using System.Collections.Generic;

namespace Soedeum.Dotnet.Library.Collections
{
    public interface ITable<TKey, TValue>
    {
        bool Define(TKey key, TValue value, bool overwrite = false);

        bool UnDefine(TKey key);

        void Clear();
        

        int Count { get; }

        bool IsEmpty { get; }


        IEnumerable<TKey> Keys { get; }

        IEnumerable<TValue> Values { get; }

        IEnumerable<KeyValuePair<TKey, TValue>> Items { get; }



        bool IsDefined(TKey key);

        TValue Resolve(TKey key);

        bool TryResolve(TKey key, out TValue result);

        bool TryResolve<TCast>(TKey key, out TCast result)
            where TCast : TValue;
    }
}