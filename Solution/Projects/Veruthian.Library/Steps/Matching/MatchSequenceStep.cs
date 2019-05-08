using System;
using System.Collections.Generic;
using Veruthian.Library.Collections.Extensions;

namespace Veruthian.Library.Steps.Matching
{
    public class MatchSequenceStep<T, S> : MatchStep<T>
        where T : IEquatable<T>
        where S : IEnumerable<T>
    {
        S sequence;

        public MatchSequenceStep(S sequence) => this.sequence = sequence;

        public S Sequence => sequence;

        public override string Description => $"match-sequence<{sequence.ToListString("", "", ",")}>";

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