namespace Veruthian.Library.Steps.Handlers
{
    public interface IStepHandler
    {
        bool? Handle(IStep step, StateTable state, IStepHandler root = null);
    }
}