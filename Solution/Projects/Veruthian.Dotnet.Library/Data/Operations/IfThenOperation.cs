namespace Veruthian.Dotnet.Library.Data.Operations
{
    public class IfThenOperation<TState> : DoubleOperation<TState>
    {
        protected IfThenOperation(IOperation<TState> ifOperation, IOperation<TState> thenOperation)
            : base(ifOperation, thenOperation) { }

        public override string Description => "IfThen";

        protected override bool DoAction(TState state, IOperationTracer<TState> tracer = null)
        {
            if (First.Perform(state, tracer))
                return Second.Perform(state, tracer);
            else
                return false;
        }
    }
}