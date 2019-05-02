namespace Veruthian.Library.Operations
{
    public class RepetitionOperation<TState> : BaseNestedOperation<TState>
    {
        int min;

        int? max;


        public RepetitionOperation(IOperation<TState> operation, int min = 0, int? max = null) : base(operation)
        {
            this.min = min;
            this.max = max;
        }

        public override string Description => $"{GetPreface()} {Operation}";


        private string GetPreface()
        {
            if (max == null)
                if (min == 0)
                    return "repeat";
                else
                    return $"at least {min}";
            else if (min == 0)
                return $"at most {max}";
            else if (min == max)
                return $"exactly {min}";
            else
                return $"from {min} to {max}";
        }

        protected override bool DoAction(TState state, ITracer<TState> tracer = null)
        {
            return false;
        }


        public static RepetitionOperation<TState> Repeat(IOperation<TState> operation) => new RepetitionOperation<TState>(operation, 0);

        public static RepetitionOperation<TState> AtMost(int value, IOperation<TState> operation) => new RepetitionOperation<TState>(operation, 0, value);

        public static RepetitionOperation<TState> AtLeast(int value, IOperation<TState> operation) => new RepetitionOperation<TState>(operation, value);

        public static RepetitionOperation<TState> Exactly(int value, IOperation<TState> operation) => new RepetitionOperation<TState>(operation, value, value);

        public static RepetitionOperation<TState> FromTo(int min, int max, IOperation<TState> operation) => new RepetitionOperation<TState>(operation, min, max);
    }
}