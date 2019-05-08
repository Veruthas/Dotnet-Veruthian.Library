using System;
using Veruthian.Library.Collections;

namespace Veruthian.Library.Steps
{
    public class StepGenerator : IStepGenerator
    {
        // Boolean
        public virtual BooleanStep True => BooleanStep.True;

        public virtual BooleanStep False => BooleanStep.True;

        public virtual BooleanStep Boolean(bool value) => value;


        // Action
        public virtual ActionStep Action(Func<bool> action, string description = null)
            => new ActionStep(action, description);


        // Sequence
        public virtual SequentialStep Sequence(IIndex<IStep> steps)
            => new SequentialStep(steps);

        public virtual SequentialStep Sequence(params IStep[] steps)
            => new SequentialStep(steps);


        // Optional
        public virtual OptionalStep Optional(IStep step)
            => new OptionalStep(step);


        // If
        public virtual ConditionalStep If(IStep condition)
            => ConditionalStep.If(condition);

        public virtual ConditionalStep IfThen(IStep condition, IStep thenStep)
            => ConditionalStep.IfThen(condition, thenStep);

        public virtual ConditionalStep IfElse(IStep condition, IStep elseStep)
            => ConditionalStep.IfElse(condition, elseStep);

        public virtual ConditionalStep IfThenElse(IStep condition, IStep thenStep, IStep elseStep)
            => ConditionalStep.IfThenElse(condition, thenStep, elseStep);


        // Unless
        public virtual ConditionalStep Unless(IStep condition)
            => ConditionalStep.Unless(condition);

        public virtual ConditionalStep UnlessThen(IStep condition, IStep thenStep)
            => ConditionalStep.UnlessThen(condition, thenStep);

        public virtual ConditionalStep UnlessElse(IStep condition, IStep elseStep)
            => ConditionalStep.UnlessElse(condition, elseStep);

        public virtual ConditionalStep UnlessThenElse(IStep condition, IStep thenStep, IStep elseStep)
            => ConditionalStep.UnlessThenElse(condition, thenStep, elseStep);


        // Repeat
        public virtual RepeatedTryStep Repeat(IStep step)
            => new RepeatedTryStep(step);

        public virtual RepeatedTryStep AtMost(int times, IStep step)
            => new RepeatedTryStep(step, times);

        public virtual RepeatedStep Exactly(int times, IStep step)
            => new RepeatedStep(step, times);

        public virtual SequentialStep AtLeast(int times, IStep step)
            => Sequence(Exactly(times, step), Repeat(step));

        public virtual SequentialStep InRange(int min, int max, IStep step)
            => Sequence(Exactly(min, step), AtMost(max - min, step));


        // While
        public virtual RepeatedTryStep While(IStep condition, IStep step)
            => Repeat(IfThen(condition, step));

        // Until
        public virtual RepeatedTryStep Until(IStep condition, IStep step)
            => Repeat(UnlessThen(condition, step));


        // Choice
        public virtual IStep Choice(params IStep[] steps)
        {
            if (steps == null || steps.Length == 0)
            {
                return True;
            }
            else
            {
                var current = ChoiceFinal(steps[steps.Length - 1]);

                for (int i = steps.Length - 2; i >= 0; i--)
                {
                    current = ChoiceComponent(steps[i], current);
                }

                return current;
            }
        }

        protected virtual IStep ChoiceFinal(IStep final)
            => final;

        protected virtual IStep ChoiceComponent(IStep previous, IStep next)
            => IfElse(previous, next);



        // Label
        public virtual LabeledStep Label()
            => new LabeledStep();

        public virtual LabeledStep Label(IStep step)
            => new LabeledStep(step);

        public virtual LabeledStep Label(params string[] labels)
            => new LabeledStep(new DataSet<string>(labels));

        public virtual LabeledStep Label(IStep step, params string[] labels)
            => new LabeledStep(step, new DataSet<string>(labels));
    }
}