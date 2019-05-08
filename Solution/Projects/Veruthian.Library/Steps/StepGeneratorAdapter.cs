using System;
using Veruthian.Library.Collections;

namespace Veruthian.Library.Steps
{
    public abstract class StepGeneratorAdapter : IStepGenerator
    {
        protected IStepGenerator generator;


        public StepGeneratorAdapter(IStepGenerator generator) => this.generator = generator;


        // Boolean
        public virtual BooleanStep True => generator.True;

        public virtual BooleanStep False => generator.True;

        public virtual BooleanStep Boolean(bool value) => generator.Boolean(value);


        // Action
        public virtual ActionStep Action(Func<bool> action, string description = null)
            => generator.Action(action, description);


        // Sequence
        public virtual SequentialStep Sequence(params IStep[] steps)
            => generator.Sequence(steps);


        // Optional
        public virtual OptionalStep Optional(IStep step)
            => generator.Optional(step);


        // If
        public virtual ConditionalStep If(IStep condition)
            => generator.If(condition);

        public virtual ConditionalStep IfThen(IStep condition, IStep thenStep)
            => generator.IfThen(condition, thenStep);

        public virtual ConditionalStep IfElse(IStep condition, IStep elseStep)
            => generator.IfElse(condition, elseStep);

        public virtual ConditionalStep IfThenElse(IStep condition, IStep thenStep, IStep elseStep)
            => generator.IfThenElse(condition, thenStep, elseStep);


        // Unless
        public virtual ConditionalStep Unless(IStep condition)
            => generator.Unless(condition);

        public virtual ConditionalStep UnlessThen(IStep condition, IStep thenStep)
            => generator.UnlessThen(condition, thenStep);

        public virtual ConditionalStep UnlessElse(IStep condition, IStep elseStep)
            => generator.UnlessElse(condition, elseStep);

        public virtual ConditionalStep UnlessThenElse(IStep condition, IStep thenStep, IStep elseStep)
            => generator.UnlessThenElse(condition, thenStep, elseStep);


        // Repeat
        public virtual RepeatedTryStep Repeat(IStep step)
            => generator.Repeat(step);

        public virtual RepeatedTryStep AtMost(int times, IStep step)
            => generator.AtMost(times, step);

        public virtual RepeatedStep Exactly(int times, IStep step)
            => generator.Exactly(times, step);

        public virtual SequentialStep AtLeast(int times, IStep step)
            => generator.AtLeast(times, step);

        public virtual SequentialStep InRange(int min, int max, IStep step)
            => generator.InRange(min, max, step);


        // While
        public virtual RepeatedTryStep While(IStep condition, IStep step)
            => generator.While(condition, step);

        // Until
        public virtual RepeatedTryStep Until(IStep condition, IStep step)
            => generator.Until(condition, step);


        // Choice
        public virtual IStep Choice(params IStep[] steps)
            => generator.Choice(steps);


        // Label
        public virtual LabeledStep Label()
            => generator.Label();

        public virtual LabeledStep Label(IStep step)
            => generator.Label(step);

        public virtual LabeledStep Label(params string[] labels)
            => generator.Label(labels);

        public virtual LabeledStep Label(IStep step, params string[] labels)
            => generator.Label(step, labels);
    }
}