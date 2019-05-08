namespace Veruthian.Library.Steps
{
    public abstract class BaseSimpleStep : BaseStep
    {
        protected override int SubStepCount => 0;

        protected override IStep GetSubStep(int verifiedIndex) => null;
    }
}