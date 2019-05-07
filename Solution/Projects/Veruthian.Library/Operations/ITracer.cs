namespace Veruthian.Library.Operations
{
    public interface ITracer<TState>
    {
        bool? OperationStart(IOperation<TState> operation, TState state);

        void OperationFinish(IOperation<TState> operation, TState state, bool success);


        void Reset();
    }
}