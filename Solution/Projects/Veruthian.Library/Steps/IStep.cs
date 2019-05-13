using Veruthian.Library.Collections;

namespace Veruthian.Library.Steps
{
    public interface IStep
    {
        string Description { get; }

        IVector<IStep> SubSteps { get; }
    }
}