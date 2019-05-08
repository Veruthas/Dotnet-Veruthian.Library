using Veruthian.Library.Collections;
using Veruthian.Library.Collections.Extensions;

namespace Veruthian.Library.Steps.Matching
{
    public class MatchAheadSetStep<T> : MatchAheadStep<T>
    {
        IContainer<T> set;

        public MatchAheadSetStep(int lookahead, IContainer<T> set) : base(lookahead) => this.set = set;

        public override string Description => $"match-set<{LookAhead}, {set.ToListString("", "", " + ")}>";

        public override bool Match(T value) => set.Contains(value);
    }
}