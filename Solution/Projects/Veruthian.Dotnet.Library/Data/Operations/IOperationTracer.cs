namespace Veruthian.Dotnet.Library.Data.Operations
{
    public interface IOperationTracer<TState>
    {
        void StartingOperation(IOperation<TState> operation, TState state);

        void FinishingOperation(IOperation<TState> operation, TState state, bool success);
    }
}