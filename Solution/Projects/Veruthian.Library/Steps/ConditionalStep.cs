namespace Veruthian.Library.Steps
{
    public class ConditionalStep : BaseStep
    {
        IStep condition;

        bool expecting;

        IStep thenStep;

        IStep elseStep;


        public ConditionalStep(IStep condition, bool expecting = true, IStep thenStep = null, IStep elseStep = null)
        {
            this.condition = this.condition ?? (BooleanStep)!expecting;

            this.expecting = expecting;

            this.thenStep = thenStep;

            this.elseStep = elseStep;
        }


        public bool Expecting => expecting;

        public IStep Condition => condition;

        public IStep Then => thenStep;

        public IStep Else => elseStep;


        public override string Description => (expecting ? "if" : "unless") + (thenStep == null ? "" : "-then") + (elseStep == null ? "" : "-else");

        protected override int SubStepCount => 1 + (thenStep == null ? 0 : 1) + (elseStep == null ? 0 : 1);

        protected override IStep GetSubStep(int verifiedIndex)
        {
            switch (verifiedIndex)
            {
                case 0:
                    return condition;
                case 1:
                    return thenStep == null ? elseStep : thenStep;
                default:
                    return elseStep;
            }
        }



        public ConditionalStep If(IStep condition) => new ConditionalStep(condition, true);

        public ConditionalStep IfThen(IStep condition, IStep thenStep) => new ConditionalStep(condition, true, thenStep);

        public ConditionalStep IfElse(IStep condition, IStep elseStep) => new ConditionalStep(condition, true, null, elseStep);

        public ConditionalStep IfThenElse(IStep condition, IStep thenStep, IStep elseStep) => new ConditionalStep(condition, true, thenStep, elseStep);



        public ConditionalStep Unless(IStep condition) => new ConditionalStep(condition, false);

        public ConditionalStep UnlessThen(IStep condition, IStep thenStep) => new ConditionalStep(condition, false, thenStep);

        public ConditionalStep UnlessElse(IStep condition, IStep elseStep) => new ConditionalStep(condition, false, null, elseStep);

        public ConditionalStep UnlessThenElse(IStep condition, IStep thenStep, IStep elseStep) => new ConditionalStep(condition, false, thenStep, elseStep);
    }
}