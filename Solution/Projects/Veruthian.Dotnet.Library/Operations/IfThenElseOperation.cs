namespace Veruthian.Dotnet.Library.Operations
{
    public class IfThenElseOperation<TState> : TripleOperation<TState>
    {
        protected IfThenElseOperation(IOperation<TState> ifOperation, IOperation<TState> thenOperation, IOperation<TState> elseOperation)
            : base(ifOperation, thenOperation, elseOperation) { }

        public override string Description => "IfThenElse";

        protected override bool DoAction(TState state, IOperationTracer<TState> tracer = null)
        {
            if (First.Perform(state, tracer))
                return Second.Perform(state, tracer);
            else
                return Third.Perform(state, tracer);
        }
    }
}