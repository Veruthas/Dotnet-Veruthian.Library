namespace Veruthian.Library.Steps
{
    public abstract class BaseNestedStep : BaseStep
    {
        IStep step;

        public BaseNestedStep() : this(null) { }

        public BaseNestedStep(IStep step) => Step = step;

        public IStep Step
        {
            get => step;
            set => step = value ?? BooleanStep.True;
        }

        protected override int SubStepCount => 1;

        protected override IStep GetSubStep(int verifiedIndex) => step;
    }
}