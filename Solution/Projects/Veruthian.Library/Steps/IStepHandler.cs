namespace Veruthian.Library.Steps
{
    public interface IStepHandler
    {
        bool? Handle(IStepWalker walker, IStep step);
    }
}