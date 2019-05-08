using Veruthian.Library.Collections;
using Veruthian.Library.Collections.Extensions;

namespace Veruthian.Library.Steps
{
    public class LabeledStep : BaseNestedStep
    {
        IExpandableContainer<string> labels;


        public LabeledStep() { }

        public LabeledStep(IStep step) : base(step) { }

        public LabeledStep(IExpandableContainer<string> labels) => this.labels = labels;

        public LabeledStep(IStep step, IExpandableContainer<string> labels) : base(step) => this.labels = labels;


        public override string Description => labels == null ? "labeled<>" : "labeled" + labels.ToListString("<", ">");


        public IExpandableContainer<string> Labels
        {
            get => labels;
            set => labels = value;
        }        


        public bool Has(string label) => labels == null ? false : labels.Contains(label);
    }
}