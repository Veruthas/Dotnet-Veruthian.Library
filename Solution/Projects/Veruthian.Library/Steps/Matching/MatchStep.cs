namespace Veruthian.Library.Steps.Matching
{
    public abstract class MatchStep<T> : BaseSimpleStep
    {
        public abstract bool Match(T value);
    }
}