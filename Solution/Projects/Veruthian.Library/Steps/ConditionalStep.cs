namespace Veruthian.Library.Steps
{
    public class ConditionalStep : Step
    {
        IStep condition;

        bool expecting;

        IStep thenStep;

        IStep elseStep;


        public ConditionalStep(IStep condition, bool expecting = true, IStep thenStep = null, IStep elseStep = null)
        {
            this.condition = condition;

            this.expecting = expecting;

            this.thenStep = thenStep;

            this.elseStep = elseStep;
        }


        public bool Expecting => expecting;

        public IStep Condition => condition;

        public IStep Then => thenStep;

        public IStep Else => elseStep;



        private bool HasThen => Then != null;

        private bool HasElse => Else != null;


        public override string Description => (expecting ? "if" : "unless") +
                                              (HasThen ? "-then" : "") +
                                              (HasElse ? "-else" : "");

        protected override int SubStepCount => 1 +
                                              (HasThen ? 1 : 0) +
                                              (HasElse ? 1 : 0);

        protected override IStep GetSubStep(int verifiedIndex)
        {
            switch (verifiedIndex)
            {
                case 0:
                    return condition;
                case 1:
                    return HasThen ? thenStep : elseStep;
                default:
                    return elseStep;
            }
        }



        public static ConditionalStep If(IStep condition) => new ConditionalStep(condition, true);

        public static ConditionalStep IfThen(IStep condition, IStep thenStep) => new ConditionalStep(condition, true, thenStep);

        public static ConditionalStep IfElse(IStep condition, IStep elseStep) => new ConditionalStep(condition, true, null, elseStep);

        public static ConditionalStep IfThenElse(IStep condition, IStep thenStep, IStep elseStep) => new ConditionalStep(condition, true, thenStep, elseStep);



        public static ConditionalStep Unless(IStep condition) => new ConditionalStep(condition, false);

        public static ConditionalStep UnlessThen(IStep condition, IStep thenStep) => new ConditionalStep(condition, false, thenStep);

        public static ConditionalStep UnlessElse(IStep condition, IStep elseStep) => new ConditionalStep(condition, false, null, elseStep);

        public static ConditionalStep UnlessThenElse(IStep condition, IStep thenStep, IStep elseStep) => new ConditionalStep(condition, false, thenStep, elseStep);
    }
}