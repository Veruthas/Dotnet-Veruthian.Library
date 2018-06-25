namespace Veruthian.Dotnet.Library.Data.Operations
{
    public delegate bool TripleOperationAction<TState>(IOperation<TState> first, IOperation<TState> second, IOperation<TState> third, TState state, IOperationTracer<TState> tracer = null);
    public class DynamicTripleOperation<TState> : TripleOperation<TState>
    {
        TripleOperationAction<TState> action;

        string description;

        protected DynamicTripleOperation(IOperation<TState> first, IOperation<TState> second, IOperation<TState> third, 
                                        TripleOperationAction<TState> action, string description = null) 
            : base(first, second, third)
        {
            this.action = action ?? ((o0, o1, o2, s, t) => false);

            this.description = description;
        }

        public override string Description => description ?? "DynamicTripleOperation";

        protected override bool DoAction(TState state, IOperationTracer<TState> tracer = null) => action(First, Second, Third, state, tracer);
    }
}