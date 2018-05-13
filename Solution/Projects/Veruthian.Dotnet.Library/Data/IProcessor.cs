namespace Veruthian.Dotnet.Library.Data
{
    public interface IProcessor<in TState>
    {
        bool Process(TState state);
    }
}