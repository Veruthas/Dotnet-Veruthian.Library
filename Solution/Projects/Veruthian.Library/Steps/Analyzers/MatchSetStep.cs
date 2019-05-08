using Veruthian.Library.Collections;
using Veruthian.Library.Collections.Extensions;

namespace Veruthian.Library.Steps.Analyzers
{
    public class MatchSetStep<T> : MatchStep<T>
    {
        IContainer<T> set;

        public MatchSetStep(IContainer<T> set) => this.set = set;

        public override string Description => "match-set" + set.ToListString("<", ">", " + ");

        public override bool Match(T value) => set.Contains(value);
    }
}