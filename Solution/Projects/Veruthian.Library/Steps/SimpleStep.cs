namespace Veruthian.Library.Steps
{
    public abstract class SimpleStep : Step
    {
        protected override int SubStepCount => 0;

        protected override IStep GetSubStep(int verifiedAddress) => null;
    }
}