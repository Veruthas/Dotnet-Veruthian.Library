using Veruthian.Library.Collections;

namespace Veruthian.Library.Steps.Match
{
    public class MatchSetStep<T> : MatchStep<T>
    {
        public MatchSetStep(IContainer<T> set) => this.Set = set;

        public IContainer<T> Set { get; }

        protected override string MatchingTo => Set?.ToString();

        public override (bool Result, object State) Match(T value, object state = null)
        {
            if (Set == null)
                return (false, null);
            else
                return (Set.Contains(value), null);
        }            
    }
}