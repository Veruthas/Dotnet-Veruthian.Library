namespace Veruthian.Library.Operations
{
    public interface ITracer<TState>
    {
        void OnStart(IOperation<TState> operation, TState state, out bool? handled);

        void OnFinish(IOperation<TState> operation, TState state, bool success);
    }
}