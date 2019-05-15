using Veruthian.Library.Collections;
using Veruthian.Library.Collections.Extensions;

namespace Veruthian.Library.Steps
{
    public class LabeledStep : NestedStep
    {
        string name;

        IResizableContainer<string> labels;


        public LabeledStep() { }

        public LabeledStep(IResizableContainer<string> labels)
            : this(null, null, labels) { }

        public LabeledStep(string name, IResizableContainer<string> labels = null)
            : this(null, name, labels) { }

        public LabeledStep(IStep step, IResizableContainer<string> labels)
            : this(step, null, labels) { }
        
        public LabeledStep(IStep step, string name = null, IResizableContainer<string> labels = null) : base(step)
        {
            this.name = name;

            this.labels = labels;
        }


        public override string Description => $"labeled<{(name == null ? "" : name + ": ")}{(labels == null ? "" : labels.ToListString("", ""))}>";


        public string Name => name;

        public IResizableContainer<string> Labels
        {
            get => labels;
            set => labels = value;
        }


        public bool Has(string label) => labels == null ? false : labels.Contains(label);
    }
}