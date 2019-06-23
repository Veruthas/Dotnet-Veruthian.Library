using System;

namespace Veruthian.Library.Steps.Match
{
    public class MatchValueStep<T> : MatchStep<T>
        where T : IEquatable<T>
    {
        public MatchValueStep(T value) => this.Value = value;
        

        public T Value { get; }

        protected override string MatchingTo => Value?.ToString();

        public override (bool Result, object State) Match(T value, object state = null)
        {
            if (Value == null)
                return (value == null, null);
            else
                return (Value.Equals(value), null);
        }
    }
}