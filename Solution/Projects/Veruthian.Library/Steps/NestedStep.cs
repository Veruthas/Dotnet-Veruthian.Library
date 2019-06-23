namespace Veruthian.Library.Steps
{
    public class NestedStep : BaseNestedStep
    {
        public NestedStep(string description, IStep step) : base(step) => this.Description = description;

        public override string Description { get; }

        protected override IStep GetDown() => base.Step;
    }
}