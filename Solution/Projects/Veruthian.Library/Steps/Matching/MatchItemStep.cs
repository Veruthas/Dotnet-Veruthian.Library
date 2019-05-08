using System;

namespace Veruthian.Library.Steps.Matching
{
    public class MatchItemStep<T> : MatchSimpleStep<T>
        where T : IEquatable<T>
    {
        T expecting;

        public MatchItemStep(T expecting) => this.expecting = expecting;

        public override string Description => $"match<{Expecting}>";

        public T Expecting => expecting;

        protected override bool Match(T value) => (expecting == null) ? value == null : expecting.Equals(value);
    }
}