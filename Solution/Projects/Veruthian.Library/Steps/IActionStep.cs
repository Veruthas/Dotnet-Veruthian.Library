using Veruthian.Library.Collections;

namespace Veruthian.Library.Steps
{
    public interface IActionStep : IStep
    {
        bool? Act(bool? state, bool completed);
    }

    public interface IActionStep<in T> : IStep
    {
        bool? Act(bool? state, bool completed, T value);
    }
}