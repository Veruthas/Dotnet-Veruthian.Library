namespace Veruthian.Library.Operations
{
    public interface ITracer<TState>
    {
        void OnStart(IOperation<TState> operation, TState state);

        void OnFinish(IOperation<TState> operation, TState state, bool success);
    }
}