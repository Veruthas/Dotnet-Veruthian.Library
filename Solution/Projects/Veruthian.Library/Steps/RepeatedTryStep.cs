namespace Veruthian.Library.Steps
{
    public class RepeatedTryStep : BaseNestedStep
    {
        int? count;

        public RepeatedTryStep(IStep step, int? count) : base(step) => this.count = count;

        public override string Description => "repeat" + (count == null ? "" : "<" + count.ToString() + ">") + "?";
    }
}