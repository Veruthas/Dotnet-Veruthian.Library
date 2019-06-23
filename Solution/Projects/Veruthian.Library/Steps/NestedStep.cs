namespace Veruthian.Library.Steps
{
    public class NestedStep : BaseNestedStep
    {
        public NestedStep() { }

        public NestedStep(IStep down) => this.Down = down;

        public NestedStep(string description) => this.Description = description;

        public NestedStep(string description, IStep down) { this.Description = description; this.Down = down; }

        public override string Description { get; }
    }
}