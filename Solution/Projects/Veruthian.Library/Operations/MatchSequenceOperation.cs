using System;
using System.Collections.Generic;
using Veruthian.Library.Readers;
using Veruthian.Library.Types;

namespace Veruthian.Library.Operations
{
    public class MatchSequenceOperation<S, T, TState> : BaseSimpleOperation<TState>
        where TState : Has<IReader<T>>
        where T : IEquatable<T>
        where S : IEnumerable<T>
    {
        S sequence;

        public MatchSequenceOperation(S sequence) => this.sequence = sequence;

        public S Seqeunce => sequence;

        public override string Description => $"MatchSequence({sequence.ToString()})";

        protected override bool DoAction(TState state, ITracer<TState> tracer = null)
        {
            state.Get(out IReader<T> reader);

            if (!reader.IsEnd)
            {
                foreach (var item in sequence)
                {
                    var value = reader.Peek();

                    if (item.Equals(value))
                        reader.Read();
                    else
                        return false;
                }
            }

            return true;
        }
    }
}