namespace Veruthian.Library.Steps.Analyzers
{
    public abstract class MatchStep<T> : BaseSimpleStep
    {
        public abstract bool Match(T value);
    }
}