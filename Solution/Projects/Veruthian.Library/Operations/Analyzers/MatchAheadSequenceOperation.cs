using System;
using System.Collections.Generic;
using Veruthian.Library.Readers;
using Veruthian.Library.Types;
using Veruthian.Library.Utility;

namespace Veruthian.Library.Operations.Analyzers
{
    public class MatchAheadSequenceOperation<TState, TReader, T, S> : BaseLookaheadOperation<TState, TReader, T>
        where TState : Has<TReader>
        where TReader : ILookaheadReader<T>
        where T : IEquatable<T>
        where S : IEnumerable<T>
    {
        S sequence;

        public MatchAheadSequenceOperation(int lookahead, S sequence) : base(lookahead)
        {
            ExceptionHelper.VerifyNotNull(sequence);

            this.sequence = sequence;
        }

        public override string Description => $"match-sequence(+{lookahead}, {sequence})";

        public S Sequence => sequence;

        protected override bool Process(TReader reader)
        {
            foreach (var item in sequence)
            {
                if (reader.IsEnd) return false;

                var value = reader.Peek();

                if ((item == null && value == null) || item.Equals(value))
                    reader.Read();
                else
                    return false;
            }

            return true;
        }
    }
}