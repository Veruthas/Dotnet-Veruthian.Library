namespace Veruthian.Library.Steps
{
    public class OptionalStep : BaseNestedStep
    {
        public OptionalStep(IStep step) : base(step) { }

        public override string Description => "optional";
    }
}