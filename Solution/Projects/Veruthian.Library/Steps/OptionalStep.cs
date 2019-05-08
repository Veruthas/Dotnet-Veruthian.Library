namespace Veruthian.Library.Steps
{
    public class OptionalStep : NestedStep
    {
        public OptionalStep(IStep step) : base(step) { }

        public override string Description => "optional";
    }
}