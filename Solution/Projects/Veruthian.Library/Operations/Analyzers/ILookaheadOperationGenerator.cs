using System;
using System.Collections.Generic;
using Veruthian.Library.Collections;
using Veruthian.Library.Readers;
using Veruthian.Library.Types;

namespace Veruthian.Library.Operations.Analyzers
{
    public interface ILookaheadOperationGenerator<TState, TReader, T> : IMatchOperationGenerator<TState, TReader, T>
        where TState : Has<TReader>
        where TReader : ILookaheadReader<T>
        where T : IEquatable<T>
    {
        MatchAheadOperation<TState, TReader, T> Match(int lookahead, T value);

        MatchAheadSetOperation<TState, TReader, T> MatchSet(int lookahead, IContainer<T> set);

        MatchAheadSequenceOperation<TState, TReader, T, S> MatchSequence<S>(int lookahead, S sequence) where S : IEnumerable<T>;
    }
}