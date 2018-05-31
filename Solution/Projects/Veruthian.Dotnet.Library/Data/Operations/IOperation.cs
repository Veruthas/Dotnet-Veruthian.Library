namespace Veruthian.Dotnet.Library.Data.Operations
{
    public interface IOperation<TState>
    {
        bool Perform(TState state);

        bool Perform(TState state, IOperationTracer<TState> tracer);
    }
}