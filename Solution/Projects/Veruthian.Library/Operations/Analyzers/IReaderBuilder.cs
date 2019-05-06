using System;
using System.Collections.Generic;
using Veruthian.Library.Collections;
using Veruthian.Library.Readers;
using Veruthian.Library.Types;

namespace Veruthian.Library.Operations.Analyzers
{
    public interface IReaderBuilder<TState, TReader, T> : IBuilder<TState>
        where TState : Has<TReader>
        where TReader : IReader<T>
        where T : IEquatable<T>
    {
        MatchOperation<TState, TReader, T> Match(T value);
        
        MatchSetOperation<TState, TReader, T> Match(IContainer<T> set);

        MatchSequenceOperation<TState, TReader, T, S> Match<S>(S sequence) where S : IEnumerable<T>;


        ClassifiedOperation<TState> Literal(IOperation<TState> operation);
    }
}