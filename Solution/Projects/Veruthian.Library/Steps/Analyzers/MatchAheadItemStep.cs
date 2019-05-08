using System;

namespace Veruthian.Library.Steps.Analyzers
{
    public class MatchAheadItemStep<T> : MatchAheadStep<T>
        where T : IEquatable<T>
    {
        T expecting;

        public MatchAheadItemStep(int lookahead, T expecting) : base(lookahead) => this.expecting = expecting;

        public override string Description => $"match<{LookAhead}, {Expecting}>";

        public T Expecting => expecting;

        public override bool Match(T value) => (expecting == null && value == null) || expecting.Equals(value);
    }
}