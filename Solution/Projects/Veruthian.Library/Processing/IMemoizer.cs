namespace Veruthian.Library.Processing
{
    public interface IMemoizer<V>
    {
        void Memoize(V value);

        bool IsMemoized(V value);
    }

    public interface IMemoizer<V, A> : IMemoizer<V>
    {
        bool IsMemoized(V value, out A associated);
    }
}