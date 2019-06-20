namespace Veruthian.Library.Steps
{
    public abstract class BaseNestedStep : BaseStep
    {
        protected BaseNestedStep(IStep step) => Step = step;

        public IStep Step { get; }

        protected override IStep GetDown() => Step;
    }
}