namespace Veruthian.Library.Operations.Extensions
{
    public static class OperationExtensions
    {
        public static FlattenedOperation<TState>[] Flatten<TState>(this IOperation<TState> operation) 
        {
            return FlattenedOperation<TState>.Flatten(operation);
        }
    }
}