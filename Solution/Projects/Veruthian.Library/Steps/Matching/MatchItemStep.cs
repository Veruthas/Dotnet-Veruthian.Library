using System;

namespace Veruthian.Library.Steps.Matching
{
    public class MatchItemStep<T> : MatchSimpleStep<T>
        where T : IEquatable<T>
    {
        T expected;

        Func<T, string> toString;


        public MatchItemStep(T expected, Func<T, string> toString = null)
        {
            this.expected = expected;

            this.toString = toString ?? ((t) => t.ToString() ?? string.Empty);
        }

        public override string Description => $"match<{toString(expected)}>";

        public T Expected => expected;

        protected override bool Match(T value) => (expected == null) ? value == null : expected.Equals(value);
    }
}