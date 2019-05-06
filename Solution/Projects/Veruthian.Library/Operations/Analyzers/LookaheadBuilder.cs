using System;
using System.Collections.Generic;
using Veruthian.Library.Collections;
using Veruthian.Library.Readers;
using Veruthian.Library.Types;

namespace Veruthian.Library.Operations.Analyzers
{
    public class LookaheadBuilder<TState, TReader, T> : ReaderBuilder<TState, TReader, T>, ILookaheadBuilder<TState, TReader, T>
        where TState : Has<TReader>
        where TReader : ILookaheadReader<T>
        where T : IEquatable<T>
    {
        public LookaheadBuilder(IBuilder<TState> builder) : base(builder) { }

        public MatchAheadOperation<TState, TReader, T> Match(int lookahead, T value)
            => new MatchAheadOperation<TState, TReader, T>(lookahead, value);

        public MatchAheadSetOperation<TState, TReader, T> Match(int lookahead, IContainer<T> set)
            => new MatchAheadSetOperation<TState, TReader, T>(lookahead, set);

        public MatchAheadSequenceOperation<TState, TReader, T, S> Match<S>(int lookahead, S sequence) where S : IEnumerable<T>
            => new MatchAheadSequenceOperation<TState, TReader, T, S>(lookahead, sequence);
    }
}