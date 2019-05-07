using System;
using System.Collections.Generic;
using Veruthian.Library.Collections;
using Veruthian.Library.Processing;
using Veruthian.Library.Readers;
using Veruthian.Library.Types;

namespace Veruthian.Library.Operations.Analyzers
{
    public class ReaderBuilder<TState, TReader, T> : BuilderAdapter<TState>, IReaderBuilder<TState, TReader, T>
        where TState : Has<TReader>
        where TReader : IReader<T>
        where T : IEquatable<T>
    {
        public ReaderBuilder(IBuilder<TState> builder) : base(builder) { }

        // Match
        public MatchOperation<TState, TReader, T> Match(T value)
            => new MatchOperation<TState, TReader, T>(value);

        public MatchSetOperation<TState, TReader, T> MatchSet(IContainer<T> set)
            => new MatchSetOperation<TState, TReader, T>(set);

        public MatchSequenceOperation<TState, TReader, T, S> MatchSequence<S>(S sequence) where S : IEnumerable<T>
            => new MatchSequenceOperation<TState, TReader, T, S>(sequence);
    }
}