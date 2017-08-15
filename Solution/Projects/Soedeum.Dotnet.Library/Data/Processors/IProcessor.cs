namespace Soedeum.Dotnet.Library.Data.Processors
{
    public interface IProcessor<TState>
    {
        bool Process(TState state);
    }
}