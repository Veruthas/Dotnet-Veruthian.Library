using System.Collections.Generic;

namespace Veruthian.Library.Steps
{
    public class NestedStepGenerator : StepGenerator
    {
        public override IStep Sequence(params IStep[] steps)
            => new NestedStep("Seqeuence", base.Sequence(steps));

        public override IStep Sequence(IEnumerable<IStep> steps)
            => new NestedStep("Seqeuence", base.Sequence(steps));

        // Boolean
        public override IStep True
            => new NestedStep("True", base.True);

        public override IStep False
            => new NestedStep("False", base.False);

        // If
        public override IStep If(IStep condition)
            => new NestedStep("If", base.If(condition));

        public override IStep IfThen(IStep condition, IStep thenStep)
            => new NestedStep("IfThen", base.IfThen(condition, thenStep));

        public override IStep IfElse(IStep condition, IStep elseStep)
            => new NestedStep("IfElse", base.IfElse(condition, elseStep));

        public override IStep IfThenElse(IStep condition, IStep thenStep, IStep elseStep)
            => new NestedStep("IfThenElse", base.IfThenElse(condition, thenStep, elseStep));

        // Unless
        public override IStep Unless(IStep condition)
            => new NestedStep("Unless", base.Unless(condition));

        public override IStep UnlessThen(IStep condition, IStep thenStep)
            => new NestedStep("UnlessThen", base.UnlessThen(condition, thenStep));

        public override IStep UnlessElse(IStep condition, IStep elseStep)
            => new NestedStep("UnlessElse", base.UnlessElse(condition, elseStep));

        public override IStep UnlessThenElse(IStep condition, IStep thenStep, IStep elseStep)
            => new NestedStep("UnlessThenElse", base.UnlessThenElse(condition, thenStep, elseStep));


        // Repeat
        public override IStep While(IStep condition, IStep step)
            => new NestedStep("While", base.While(condition, step));

        public override IStep Until(IStep condition, IStep step)
            => new NestedStep("Until", base.Until(condition, step));

        public override IStep Exactly(int times, IStep step)
            => new NestedStep($"Exactly<{times}>", base.Exactly(times, step));

        public override IStep AtMost(int times, IStep condition, IStep step)
            => new NestedStep($"AtMost<{times}>", base.AtMost(times, condition, step));

        public override IStep AtLeast(int times, IStep condition, IStep step)
            => new NestedStep($"AtLeast<{times}>", base.AtLeast(times, condition, step));
            
        public override IStep Between(int min, int max, IStep condition, IStep step)
            => new NestedStep($"Between<{min},{max}>", base.Between(min, max, condition, step));
    }
}