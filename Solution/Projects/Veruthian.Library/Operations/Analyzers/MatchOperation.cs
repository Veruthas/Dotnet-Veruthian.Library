using System;
using Veruthian.Library.Readers;
using Veruthian.Library.Types;

namespace Veruthian.Library.Operations.Analyzers
{
    public class MatchOperation<TState, TReader, T> : BaseMatchOperation<TState, TReader, T>
        where TState : Has<TReader>
        where TReader : IReader<T>
        where T : IEquatable<T>
    {
        T value;

        public MatchOperation(T value) => this.value = value;

        public T Value => value;

        public override string Description => $"match({(value.ToString() ?? "<NULL>")})";

        protected override bool Match(T value) => this.value != null && this.value.Equals(value);
    }
}