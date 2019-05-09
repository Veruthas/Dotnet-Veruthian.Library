using System;
using System.Collections.Generic;
using Veruthian.Library.Collections.Extensions;
using Veruthian.Library.Utility;

namespace Veruthian.Library.Steps.Matching
{
    public class MatchSequenceStep<T> : MatchStep<T>
        where T : IEquatable<T>
    {
        IEnumerable<T> sequence;

        Func<IEnumerable<T>, string> toString;


        public MatchSequenceStep(IEnumerable<T> sequence, Func<IEnumerable<T>, string> toString = null)
        {
            ExceptionHelper.VerifyNotNull(sequence, nameof(sequence));

            this.sequence = sequence;

            this.toString = toString ?? ((s) => s.ToString());
        }

        public IEnumerable<T> Sequence => sequence;

        public override string Description => $"match-sequence<{toString(sequence)}>";

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