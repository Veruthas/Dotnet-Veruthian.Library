namespace Veruthian.Dotnet.Library.Data.Operations
{
    public interface IOperation<TState>
    {
        string Name { get; }
        
        bool Perform(TState state, IOperationTracer<TState> tracer = null);
    }
}