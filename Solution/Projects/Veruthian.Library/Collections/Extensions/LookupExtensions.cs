namespace Veruthian.Library.Collections.Extensions
{
    public static class LookupExtensions
    {
        public static void Get<A, V, T>(this ILookup<A, V> lookup, A address, out T result)
            where T : V
        {
            object value = lookup[address];

            result = (T)value;
        }

        public static bool TryGet<A, V, T>(this ILookup<A, V> lookup, A address, out T result)
            where T : V
        {
            object value = lookup[address];

            if (value is T)
            {
                result = (T)value;
                return true;
            }
            else
            {
                result = default(T);
                return false;
            }
        }
    }
}