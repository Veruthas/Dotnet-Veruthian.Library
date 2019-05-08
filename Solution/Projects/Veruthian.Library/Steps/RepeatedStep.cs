namespace Veruthian.Library.Steps
{
    public class RepeatedStep : BaseNestedStep
    {
        int count;

        public RepeatedStep(IStep step, int count) : base(step) { }

        public int Count => count;

        public override string Description => "repeat<" + count + ">";
    }
}