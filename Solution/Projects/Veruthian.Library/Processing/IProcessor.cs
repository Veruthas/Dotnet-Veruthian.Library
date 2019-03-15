namespace Veruthian.Library.Processing
{
    public interface IProcessor<in TState>
    {
        bool Process(TState state);
    }
}