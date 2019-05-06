using System;
using Veruthian.Library.Readers;
using Veruthian.Library.Types;

namespace Veruthian.Library.Operations.Analyzers
{
    public class MatchAheadOperation<TState, TReader, T> : BaseMatchAheadOperation<TState, TReader, T>
        where TState : Has<TReader>
        where TReader : ILookaheadReader<T>
        where T : IEquatable<T>
    {
        T value;

        public MatchAheadOperation(int lookahead, T value) : base(lookahead) => this.value = value;

        public T Value => value;

        public override string Description => $"match(+{lookahead}, {(value.ToString() ?? "<NULL>")})";

        protected override bool Match(T value) => this.value != null && this.value.Equals(value);
    }
}