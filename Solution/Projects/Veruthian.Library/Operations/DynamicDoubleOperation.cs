namespace Veruthian.Library.Operations
{
    public delegate bool DoubleOperationAction<TState>(IOperation<TState> first, IOperation<TState> second, TState state, IOperationTracer<TState> tracer = null);
    public class DynamicDoubleOperation<TState> : DoubleOperation<TState>
    {
        DoubleOperationAction<TState> action;

        string description;

        protected DynamicDoubleOperation(IOperation<TState> first, IOperation<TState> second,
                                        DoubleOperationAction<TState> action, string description = null)
            : base(first, second)
        {
            this.action = action ?? ((o0, o1, s, t) => false);

            this.description = description;
        }

        public override string Description => description ?? "DynamicDoubleOperation";

        protected override bool DoAction(TState state, IOperationTracer<TState> tracer = null) => action(First, Second, state, tracer);
    }
}