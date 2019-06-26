namespace Veruthian.Library.Steps
{
    public class LabeledStep : LinkStep
    {
        public LabeledStep(string label) => Label = label;

        public string Label { get; }

        public override string Description => Label;
    }
}