using System;
using Veruthian.Library.Readers;
using Veruthian.Library.Types;

namespace Veruthian.Library.Operations
{
    public class MatchOperation<TState, T> : BaseReadOperation<TState, T>
        where TState : Has<IReader<T>>
        where T : IEquatable<T>
    {
        T match;

        public MatchOperation(T match) => this.match = match;

        public override string Description => $"is({(match.ToString() ?? "<NULL>")})";

        protected override bool Match(T value) => match != null && match.Equals(value);
    }
}