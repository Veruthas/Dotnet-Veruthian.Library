using System;
using Veruthian.Library.Collections;

namespace Veruthian.Library.Steps
{
    public interface IStepGenerator
    {
        // Boolean
        BooleanStep True { get; }

        BooleanStep False { get; }

        BooleanStep Boolean(bool value);


        // Action
        ActionStep Action(Func<bool> action, string description = null);


        // Sequence
        SequentialStep Sequence(IIndex<IStep> steps);

        SequentialStep Sequence(params IStep[] steps);


        // Optional
        OptionalStep Optional(IStep step);


        // If
        ConditionalStep If(IStep condition);

        ConditionalStep IfThen(IStep condition, IStep thenStep);

        ConditionalStep IfElse(IStep condition, IStep elseStep);

        ConditionalStep IfThenElse(IStep condition, IStep thenStep, IStep elseStep);


        // Unless
        ConditionalStep Unless(IStep condition);

        ConditionalStep UnlessThen(IStep condition, IStep thenStep);

        ConditionalStep UnlessElse(IStep condition, IStep elseStep);

        ConditionalStep UnlessThenElse(IStep condition, IStep thenStep, IStep elseStep);


        // Repeat
        RepeatedTryStep Repeat(IStep step);

        RepeatedTryStep AtMost(int times, IStep step);

        RepeatedStep Exactly(int times, IStep step);

        SequentialStep AtLeast(int times, IStep step);

        SequentialStep InRange(int min, int max, IStep step);


        // While
        RepeatedTryStep While(IStep condition, IStep step);

        // Until
        RepeatedTryStep Until(IStep condition, IStep step);


        // Choice
        IStep Choice(params IStep[] steps);


        // Label
        LabeledStep Label();

        LabeledStep Label(IStep step);

        LabeledStep Label(params string[] labels);

        LabeledStep Label(IStep step, params string[] labels);
    }
}