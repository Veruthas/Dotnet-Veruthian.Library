namespace Veruthian.Dotnet.Library.Data.Operations
{
    public class BooleanOperation<TState> : IOperation<TState>
    {
        bool value;

        private BooleanOperation(bool value) => this.value = value;

        public bool Perform(TState state)
        {
            return value;
        }


        public string Name => value.ToString();

        public bool Perform(TState state, IOperationTracer<TState> tracer)
        {
            if (tracer != null)
            {
                tracer.StartingOperation(this, state);

                tracer.FinishingOperation(this, state, value);
            }

            return value;
        }

        public override string ToString() => value.ToString();


        public static readonly BooleanOperation<TState> True = new BooleanOperation<TState>(true);
        public static readonly BooleanOperation<TState> False = new BooleanOperation<TState>(false);
    }
}