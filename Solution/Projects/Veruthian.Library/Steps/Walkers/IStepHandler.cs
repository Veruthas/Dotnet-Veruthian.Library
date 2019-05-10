namespace Veruthian.Library.Steps.Walkers
{
    public interface IStepHandler<TState>
    {
        bool? Handle(IStep step, IStepWalker<TState> walker, TState state);
    }
}