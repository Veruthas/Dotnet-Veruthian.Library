namespace Veruthian.Library.Steps
{
    public abstract class BaseNestedStep : BaseStep
    {
        protected BaseNestedStep() { }

        protected BaseNestedStep(IStep step) => Step = step;

        public IStep Step { get; set; }

        protected override IStep GetDown() => Step;
    }
}