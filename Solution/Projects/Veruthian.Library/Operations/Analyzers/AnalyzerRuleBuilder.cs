using System;
using Veruthian.Library.Numeric;
using Veruthian.Library.Processing;
using Veruthian.Library.Readers;
using Veruthian.Library.Types;

namespace Veruthian.Library.Operations.Analyzers
{
    public class AnalyzerRuleBuilder<TState, TReader, T> : SpeculativeOperationBuilder<TState>
        where TState : Has<TReader>, Has<ISpeculative>
        where TReader : ISpeculativeReader<T>
        where T : IEquatable<T>, ISequential<T>, IBounded<T>
    {
        public MatchOperation<TState, TReader, T> Match(T value) => new MatchOperation<TState, TReader, T>(value);

        public MatchSetOperation<TState, TReader, T> MatchSet(RangeSet<T> set) => new MatchSetOperation<TState, TReader, T>(set);

        public MatchSetOperation<TState, TReader, T> MatchSequence(RangeSet<T> set) => new MatchSetOperation<TState, TReader, T>(set);    
    }
}