namespace Veruthian.Library.Steps
{
    public class NestedStep : BaseNestedStep
    {
        public NestedStep() { }

        public NestedStep(IStep step) => this.Down = step;

        public NestedStep(string description) => this.Description = description;

        public NestedStep(string description, IStep step) { this.Description = description; this.Down = step; }

        public override string Description { get; }
    }
}