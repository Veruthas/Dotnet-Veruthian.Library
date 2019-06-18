using Veruthian.Library.Collections;

namespace Veruthian.Library.Steps
{
    public interface IActionStep : IStep
    {
        void Act(StepWalker walker, ObjectTable states);
    }
}