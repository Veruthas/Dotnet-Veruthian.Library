namespace Veruthian.Library.Steps.Handlers
{
    public interface IStepHandler<TState>
    {
        bool? Handle(IStep step, TState state, IStepHandler<TState> root = null);
    }
}