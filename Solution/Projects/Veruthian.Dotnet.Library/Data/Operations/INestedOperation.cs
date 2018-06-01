namespace Veruthian.Dotnet.Library.Data.Operations
{
    public interface INestedOperation<TState> : IOperation<TState>
    {
        IOperation<TState> Operation { get; }
    }
}