namespace Veruthian.Library.Steps
{
    public abstract class NestedStep : Step
    {
        IStep step;

        public NestedStep() : this(null) { }

        public NestedStep(IStep step) => Step = step;

        public IStep Step
        {
            get => step;
            set => step = value ?? BooleanStep.True;
        }

        protected override int SubStepCount => 1;

        protected override IStep GetSubStep(int verifiedIndex) => step;
    }
}