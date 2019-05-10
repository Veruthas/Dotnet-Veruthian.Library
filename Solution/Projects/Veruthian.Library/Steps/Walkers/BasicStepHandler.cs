using System;

namespace Veruthian.Library.Steps.Walkers
{
    public class BasicStepHandler<TState> : IStepHandler<TState>
    {
        public virtual bool? Handle(IStep step, IStepWalker<TState> walker, TState state)
        {
            switch (step)
            {
                case BooleanStep boolean:
                    return HandleBooleanStep(boolean, walker, state);

                case SequentialStep sequence:
                    return HandleSequentialStep(sequence, walker, state);

                case OptionalStep optional:
                    return HandleOptionalStep(optional, walker, state);

                case RepeatedStep repeated:
                    return HandleRepeatedStep(repeated, walker, state);

                case RepeatedTryStep repeated:
                    return HandleRepeatedTryStep(repeated, walker, state);

                case LabeledStep labeled:
                    return HandleLabeledStep(labeled, walker, state);

                case ConditionalStep conditional:
                    return HandleConditionalStep(conditional, walker, state);

                default:
                    return null;
            }
        }

        protected virtual bool HandleBooleanStep(BooleanStep boolean, IStepWalker<TState> walker, TState state)
            => boolean.Value;

        protected virtual bool HandleSequentialStep(SequentialStep step, IStepWalker<TState> walker, TState state)
        {
            foreach (var substep in step.Sequence)
                if (!walker.Walk(substep, state))
                    return false;

            return true;
        }

        protected virtual bool HandleOptionalStep(OptionalStep optional, IStepWalker<TState> walker, TState state)
        {
            walker.Walk(optional.Step, state);

            return true;
        }

        protected virtual bool HandleRepeatedStep(RepeatedStep repeated, IStepWalker<TState> walker, TState state)
        {
            for (int i = 0; i < repeated.Count; i++)
                if (!walker.Walk(repeated.Step, state))
                    return false;

            return true;
        }

        protected virtual bool HandleRepeatedTryStep(RepeatedTryStep repeated, IStepWalker<TState> walker, TState state)
        {
            if (repeated.Count == null)
            {
                while (walker.Walk(repeated.Step, state))
                    ;

                return true;
            }
            else
            {
                for (int i = 0; i < repeated.Count; i++)
                    if (!walker.Walk(repeated.Step, state))
                        break;

                return true;
            }
        }

        protected virtual bool HandleLabeledStep(LabeledStep labeled, IStepWalker<TState> walker, TState state)
        {
            return walker.Walk(labeled.Step,state);
        }

        protected virtual bool HandleConditionalStep(ConditionalStep conditional, IStepWalker<TState> walker, TState state)
        {
            var result = HandleSpeculation(conditional.Condition, walker, state);

            if (result == conditional.Expecting)
            {
                if (conditional.HasThenStep)
                    return walker.Walk(conditional.Then, state);
                else
                    return true;
            }
            else
            {
                if (conditional.HasElseStep)
                    return walker.Walk(conditional.Else, state);
                else
                    return false;
            }

        }

        protected virtual bool HandleSpeculation(IStep speculation, IStepWalker<TState> walker, TState state)
        {
            OnSpeculationStarted(speculation, walker, state);

            var result = walker.Walk(speculation, state);

            OnSpeculationCompleted(speculation, walker, state, result);

            return result;
        }


        protected virtual void OnSpeculationStarted(IStep speculation, IStepWalker<TState> walker, TState state) { }

        protected virtual void OnSpeculationCompleted(IStep speculation, IStepWalker<TState> walker, TState state, bool result) { }
    }
}