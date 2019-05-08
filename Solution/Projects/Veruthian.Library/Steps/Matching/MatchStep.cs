namespace Veruthian.Library.Steps.Matching
{
    public abstract class MatchStep<T> : SimpleStep
    {
        public abstract (bool Success, object State) Match(T value, object state = null);
    }
}