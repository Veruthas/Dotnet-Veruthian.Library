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
    }
}