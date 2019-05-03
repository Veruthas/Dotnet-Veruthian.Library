using System;
using System.Collections.Generic;
using Veruthian.Library.Readers;
using Veruthian.Library.Types;
using Veruthian.Library.Utility;

namespace Veruthian.Library.Operations
{
    public class MatchSequenceOperation<TState, T, S> : BaseReaderOperation<TState, T>
        where TState : Has<IReader<T>>
        where T : IEquatable<T>
        where S : IEnumerable<T>
    {
        S sequence;

        public MatchSequenceOperation(S sequence)
        {
            ExceptionHelper.VerifyNotNull(sequence);

            this.sequence = sequence;
        }

        public override string Description => $"match({(sequence)})";

        public S Sequence => sequence;

        protected override bool Process(IReader<T> reader)
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