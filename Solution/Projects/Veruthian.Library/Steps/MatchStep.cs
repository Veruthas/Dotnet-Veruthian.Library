namespace Veruthian.Library.Steps
{
    public abstract class MatchStep<T> : SimpleStep
    {
        public abstract (bool Success, object State) Match(T value, object state = null);
    }
}