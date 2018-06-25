namespace Veruthian.Dotnet.Library
{
    public interface IProcessor<in TState>
    {
        bool Process(TState state);
    }
}