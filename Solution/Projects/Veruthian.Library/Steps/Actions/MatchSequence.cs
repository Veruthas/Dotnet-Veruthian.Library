using System;
using System.Collections.Generic;

namespace Veruthian.Library.Steps.Actions
{
    public class MatchSequence<T> : MatchStep<T>
        where T : IEquatable<T>
    {
        public MatchSequence(IEnumerable<T> sequence) => this.Sequence = sequence;

        public IEnumerable<T> Sequence { get; }

        protected override string MatchingTo => Sequence?.ToString();

        public override (bool Result, object State) Match(T item, object state = null)
        {
            var enumerator = state as IEnumerator<T>;

            if (enumerator == null)
                enumerator = Sequence.GetEnumerator();

            if (enumerator.MoveNext())
            {
                var expecting = enumerator.Current;

                return ((expecting == null ? item == null : expecting.Equals(item)), enumerator);
            }
            else
            {
                return (true, null);
            }
        }
    }
}