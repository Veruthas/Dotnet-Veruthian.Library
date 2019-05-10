namespace Veruthian.Library.Steps.Walkers
{
    public interface IStepWalker<TState>
    {
        bool Walk(IStep step, TState state);
    }
}