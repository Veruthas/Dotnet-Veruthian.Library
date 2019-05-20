using Veruthian.Library.Numeric;

namespace Veruthian.Library.Steps
{
    public abstract class SimpleStep : Step
    {
        protected override Number SubStepCount => Number.Zero;

        protected override IStep GetSubStep(Number verifiedAddress) => null;
    }
}