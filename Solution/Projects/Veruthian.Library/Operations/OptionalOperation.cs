namespace Veruthian.Library.Operations
{
    public class OptionalOperation<TState> : BaseNestedOperation<TState>
    {
        public OptionalOperation(IOperation<TState> operation) : base(operation) { }

        public override string Description => "optional";

        protected override bool DoAction(TState state, ITracer<TState> tracer = null)
        {
            Operation.Perform(state, tracer);

            return true;
        }
    }
}