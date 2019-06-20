using System;

namespace Veruthian.Library.Steps.Actions
{
    public class MatchItemStep<T> : MatchStep<T>
        where T : IEquatable<T>
    {
        public MatchItemStep(T value)
        {
            this.Value = value;
        }

        public MatchItemStep(string name, T value)
        {
            this.Name = name;

            this.Value = value;
        }

        public override string Type => $"MatchItem";

        public override string Name { get; }

        public T Value { get; }

        public override (bool Result, object State) Match(T item, object state = null)
        {
            if (Value == null)
                return (item == null, null);
            else
                return (Value.Equals(item), null);
        }



        public override string ToString() => StepString($"{Type}<{Value.ToString() ?? "NULL"}>", Name);
    }
}