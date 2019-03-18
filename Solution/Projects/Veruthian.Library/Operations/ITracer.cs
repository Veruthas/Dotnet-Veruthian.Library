namespace Veruthian.Library.Operations
{
    public interface ITracer<TState>
    {
        void OnStarting(IOperation<TState> operation, TState state);

        void OnFinishing(IOperation<TState> operation, TState state, bool success);
    }
}