using System.Collections.Generic;

namespace Soedeum.Dotnet.Library.Collections
{
    public static class MapExtensions
    {
        public static bool TryGetAnyValue<TKey, TValue>(
            this IDictionary<TKey, TValue> dictionary, TKey key, out TValue value, TValue defaultValue = default(TValue))
        {
            var result = dictionary.TryGetValue(key, out value);

            if (!result)
                value = defaultValue;

            return result;
        }

        public static bool TryGetAnyValue<TKey, TValue, TSubType>(
            this IDictionary<TKey, TValue> dictionary, TKey key, out TSubType value, TSubType defaultValue = default(TSubType))
            where TSubType : TValue
        {
            value = defaultValue;

            var result = dictionary.TryGetValue(key, out TValue baseValue);

            if (result && baseValue is TSubType)
                value = (TSubType)baseValue;
            else
                result = false;

            return result;
        }

        public static TValue Resolve<TKey, TValue>(this IAnnotated<TKey, TValue> annotated, TKey key, TValue defaultValue = default(TValue))
        {
            var result = annotated.TryResolve(key, out TValue value);

            return result ? value : defaultValue;
        }

        public static bool TryResolve<TKey, TValue, TCast>(
            this ITable<TKey, TValue> annotated, TKey key, out TCast value, TCast defaultValue = default(TCast))
        {

        }
    }
}