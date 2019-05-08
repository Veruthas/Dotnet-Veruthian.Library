namespace Veruthian.Library.Steps
{
    public class RepeatedTryStep : NestedStep
    {
        int? count;

        public RepeatedTryStep(IStep step, int? count = null) : base(step) => this.count = count;

        public override string Description => "repeat" + (count == null ? "" : "<" + count.ToString() + ">") + "?";
    }
}