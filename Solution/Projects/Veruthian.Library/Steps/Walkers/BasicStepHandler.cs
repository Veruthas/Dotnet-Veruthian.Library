using System;

namespace Veruthian.Library.Steps.Walkers
{
    public class BasicStepHandler : IStepHandler
    {
        public virtual bool? Handle(IStepWalker walker, IStep step)
        {
            switch (step)
            {
                case BooleanStep boolean:
                    return HandleBooleanStep(walker, boolean);

                case SequentialStep sequence:
                    return HandleSequentialStep(walker, sequence);

                case OptionalStep optional:
                    return HandleOptionalStep(walker, optional);

                case RepeatedStep repeated:
                    return HandleRepeatedStep(walker, repeated);

                case RepeatedTryStep repeated:
                    return HandleRepeatedTryStep(walker, repeated);

                case LabeledStep labeled:
                    return HandleLabeledStep(walker, labeled);

                case ConditionalStep conditional:
                    return HandleConditionalStep(walker,conditional);

                default:
                    return null;
            }
        }        

        protected virtual bool HandleBooleanStep(IStepWalker walker, BooleanStep boolean)
            => boolean.Value;

        protected virtual bool HandleSequentialStep(IStepWalker walker, SequentialStep step)
        {
            foreach (var substep in step.Sequence)
                if (!walker.Walk(substep))
                    return false;

            return true;
        }

        protected virtual bool HandleOptionalStep(IStepWalker walker, OptionalStep optional)
        {
            walker.Walk(optional.Step);

            return true;
        }

        protected virtual bool HandleRepeatedStep(IStepWalker walker, RepeatedStep repeated)
        {
            for (int i = 0; i < repeated.Count; i++)
                if (!walker.Walk(repeated.Step))
                    return false;

            return true;
        }

        protected virtual bool HandleRepeatedTryStep(IStepWalker walker, RepeatedTryStep repeated)
        {
            if (repeated.Count == null)
            {
                while (walker.Walk(repeated.Step))
                    ;

                return true;
            }
            else
            {
                for (int i = 0; i < repeated.Count; i++)
                    if (!walker.Walk(repeated.Step))
                        break;

                return true;
            }
        }

        protected virtual bool HandleLabeledStep(IStepWalker walker, LabeledStep labeled)
        {
            return walker.Walk(labeled.Step);
        }

        protected virtual bool HandleConditionalStep(IStepWalker walker, ConditionalStep conditional)
        {
            var result = HandleSpeculation(walker, conditional.Condition);

            if (result == conditional.Expecting)
            {
                if (conditional.HasThenStep)
                    return walker.Walk(conditional.Then);
                else
                    return true;
            }
            else
            {
                if (conditional.HasElseStep)
                    return walker.Walk(conditional.Else);
                else
                    return false;
            }

        }

        protected virtual bool HandleSpeculation(IStepWalker walker, IStep speculation)
        {
            OnSpeculationStarted(walker, speculation);

            var result = walker.Walk(speculation);

            OnSpeculationCompleted(walker, speculation, result);

            return result;
        }


        protected virtual void OnSpeculationStarted(IStepWalker walker, IStep speculation) { }

        protected virtual void OnSpeculationCompleted(IStepWalker walker, IStep speculation, bool result) { }
    }
}