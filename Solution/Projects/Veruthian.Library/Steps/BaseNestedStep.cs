namespace Veruthian.Library.Steps
{
    public abstract class BaseNestedStep : BaseStep
    {
        protected BaseNestedStep(IStep step) => Down = step;

        public IStep Down { get; }

        protected override IStep GetDown() => Down;
    }
}