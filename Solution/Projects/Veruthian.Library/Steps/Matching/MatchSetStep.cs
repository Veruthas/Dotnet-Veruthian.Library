using Veruthian.Library.Collections;
using Veruthian.Library.Collections.Extensions;

namespace Veruthian.Library.Steps.Matching
{
    public class MatchSetStep<T> : MatchSimpleStep<T>
    {
        IContainer<T> set;

        public MatchSetStep(IContainer<T> set) => this.set = set;

        public override string Description => "match-set" + set.ToListString("<", ">", " + ");

        protected  override bool Match(T value) => set.Contains(value);
    }
}