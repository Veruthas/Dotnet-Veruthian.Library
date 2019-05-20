using Veruthian.Library.Numeric;

namespace Veruthian.Library.Steps
{
    public abstract class NestedStep : Step
    {
        IStep step;

        public NestedStep() : this(null) { }

        public NestedStep(IStep step) => this.step = step;

        public IStep Step
        {
            get => step;
            set => step = value;
        }

        protected override Number SubStepCount => 1;

        protected override IStep GetSubStep(Number verifiedAddress) => step;
    }
}