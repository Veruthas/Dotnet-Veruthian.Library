using System;
using System.Collections.Generic;

namespace Veruthian.Library.Steps.Matching
{
    public class MatchAheadSequenceStep<T, S> : MatchAheadMultipleStep<T>
        where T : IEquatable<T>
        where S : IEnumerable<T>
    {
        S sequence;

        public MatchAheadSequenceStep(int lookahead, S sequence) : base(lookahead) => this.sequence = sequence;

        public override (bool Success, object State) Match(T value, object state = null)
        {
            var enumerator = state as IEnumerator<T>;

            if (enumerator == null)
                enumerator = sequence.GetEnumerator();

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