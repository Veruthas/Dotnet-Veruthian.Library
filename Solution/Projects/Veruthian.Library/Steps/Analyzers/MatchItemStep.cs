using System;

namespace Veruthian.Library.Steps.Analyzers
{
    public class MatchItemStep<T> : MatchStep<T>
        where T : IEquatable<T>
    {
        T expecting;

        public MatchItemStep(T expecting) => this.expecting = expecting;

        public override string Description => $"match<{Expecting}>";

        public T Expecting => expecting;

        public override bool Match(T value) => (expecting == null && value == null) || expecting.Equals(value);
    }
}