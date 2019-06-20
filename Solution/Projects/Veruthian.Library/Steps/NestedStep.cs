namespace Veruthian.Library.Steps
{
    public class NestedStep : BaseNestedStep
    {
        public NestedStep(string type, IStep step)
            : base(step)
        {
            this.Type = type;
        }

        public override string Type { get; }

        protected override IStep GetDown() => base.Step;
    }
}