namespace Veruthian.Library
{
    public interface IProcessor<in TState>
    {
        bool Process(TState state);
    }
}