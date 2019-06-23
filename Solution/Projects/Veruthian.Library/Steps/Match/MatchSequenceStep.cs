using System;
using System.Collections.Generic;

namespace Veruthian.Library.Steps.Match
{
    public class MatchSequenceStep<T> : MatchStep<T>
        where T : IEquatable<T>
    {
        public MatchSequenceStep(IEnumerable<T> sequence) => this.Sequence = sequence;

        public IEnumerable<T> Sequence { get; }

        protected override string MatchingTo => Sequence?.ToString();

        public override (bool Result, object State) Match(T value, object state = null)
        {
            var enumerator = state as IEnumerator<T>;

            if (enumerator == null)
                enumerator = Sequence.GetEnumerator();

            if (enumerator.MoveNext())
            {
                var expecting = enumerator.Current;

                return ((expecting == null ? value == null : expecting.Equals(value)), enumerator);
            }
            else
            {
                return (true, null);
            }
        }
    }
}