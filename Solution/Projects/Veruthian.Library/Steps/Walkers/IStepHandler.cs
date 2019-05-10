namespace Veruthian.Library.Steps.Walkers
{
    public interface IStepHandler
    {
        bool? Handle(IStepWalker walker, IStep step);
    }
}