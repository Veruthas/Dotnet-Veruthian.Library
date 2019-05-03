using System;
using Veruthian.Library.Readers;
using Veruthian.Library.Types;

namespace Veruthian.Library.Operations
{
    public class MatchOperation<TState, T> : BaseMatchOperation<TState, T>
        where TState : Has<IReader<T>>
        where T : IEquatable<T>
    {
        T value;

        public MatchOperation(T value) => this.value = value;

        public T Value => value;

        public override string Description => $"match({(value.ToString() ?? "<NULL>")})";

        protected override bool Match(T value) => this.value != null && this.value.Equals(value);
    }
}