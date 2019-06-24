namespace Veruthian.Library.Steps
{
    public abstract class BaseNestedStep : BaseContainedStep
    {
        public override IStep Step { get; set; }

        protected override IStep GetDown() => Step;
    }
}