using System;

namespace Veruthian.Library.Steps.Actions
{
    public class MatchItemStep<T> : MatchStep<T>
        where T : IEquatable<T>
    {
        public MatchItemStep(T value) => this.Value = value;

        public MatchItemStep(string name, T value) : base(name) => this.Value = value;
        

        public T Value { get; }

        protected override string MatchingTo => Value?.ToString();

        public override (bool Result, object State) Match(T item, object state = null)
        {
            if (Value == null)
                return (item == null, null);
            else
                return (Value.Equals(item), null);
        }
    }
}