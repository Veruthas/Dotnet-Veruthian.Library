namespace Veruthian.Library.Steps
{
    public abstract class MatchSimpleStep<T> : MatchStep<T>
    {
        public override (bool Success, object State) Match(T value, object state = null) => (Match(value), null);

        protected abstract bool Match(T value);
    }
}