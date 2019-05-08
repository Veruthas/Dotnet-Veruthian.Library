namespace Veruthian.Library.Steps
{
    public class RepeatedStep : NestedStep
    {
        int count;

        public RepeatedStep(IStep step, int count) : base(step) => this.count = count;

        public int Count => count;

        public override string Description => "repeat<" + count + ">";
    }
}