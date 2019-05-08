using Veruthian.Library.Collections;

namespace Veruthian.Library.Steps
{
    public interface IStep
    {
        string Description { get; }

        IIndex<IStep> SubSteps { get; }
    }
}