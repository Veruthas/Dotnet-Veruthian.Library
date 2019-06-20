using Veruthian.Library.Collections;

namespace Veruthian.Library.Steps.Actions
{
    public class MatchSetStep<T> : MatchStep<T>
    {
        public MatchSetStep(IContainer<T> set) => this.Set = set;

        public MatchSetStep(string name, IContainer<T> set) :base(name) => this.Set = set;

        public IContainer<T> Set { get; }

        protected override string MatchingTo => Set?.ToString();

        public override (bool Result, object State) Match(T item, object state = null)
        {
            if (Set == null)
                return (false, null);
            else
                return (Set.Contains(item), null);
        }            
    }
}