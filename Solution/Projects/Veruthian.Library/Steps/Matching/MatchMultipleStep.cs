namespace Veruthian.Library.Steps.Matching
{
    public abstract class MatchMultipleStep<T>
    {
        public abstract (bool Success, object State) Match(T value, object state = null);
    }
}