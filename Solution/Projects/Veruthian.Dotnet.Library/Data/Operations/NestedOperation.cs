namespace Veruthian.Dotnet.Library.Data.Operations
{
    public class NestedOperation<TState> : IOperation<TState>
    {
        protected readonly IOperation<TState> operation;


        public bool Perform(TState state)
        {
            throw new System.NotImplementedException();
        }

        public bool Perform(TState state, IOperationTracer<TState> tracer)
        {
            throw new System.NotImplementedException();
        }
    }
}