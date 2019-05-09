using System;

namespace Veruthian.Library.Steps
{
    public class SimpleWalker : BaseWalker
    {
        protected override bool Handle(IStep step)
        {
            switch (step)
            {
                case BooleanStep boolean:
                    return HandleBooleanStep(boolean);

                case SequentialStep sequence:
                    return HandleSequentialStep(sequence);

                case OptionalStep optional:
                    return HandleOptionalStep(optional);

                case RepeatedStep repeated:
                    return HandleRepeatedStep(repeated);

                case RepeatedTryStep repeated:
                    return HandleRepeatedTryStep(repeated);

                case LabeledStep labeled:
                    return HandleLabeledStep(labeled);

                case ConditionalStep conditional:
                    return HandleConditionalStep(conditional);

                default:
                    return false;
            }
        }

        protected virtual bool HandleBooleanStep(BooleanStep boolean)
            => boolean.Value;

        protected virtual bool HandleSequentialStep(SequentialStep step)
        {
            foreach (var substep in step.Sequence)
                if (!Walk(substep))
                    return false;

            return true;
        }

        protected virtual bool HandleOptionalStep(OptionalStep optional)
        {
            Walk(optional.Step);

            return true;
        }

        protected virtual bool HandleRepeatedStep(RepeatedStep repeated)
        {
            for (int i = 0; i < repeated.Count; i++)
                if (!Walk(repeated.Step))
                    return false;

            return true;
        }

        protected virtual bool HandleRepeatedTryStep(RepeatedTryStep repeated)
        {
            if (repeated.Count == null)
            {
                while (Walk(repeated.Step))
                    ;

                return true;
            }
            else
            {
                for (int i = 0; i < repeated.Count; i++)
                    if (!Walk(repeated.Step))
                        break;

                return true;
            }
        }

        protected virtual bool HandleLabeledStep(LabeledStep labeled)
        {
            return Walk(labeled.Step);
        }

        protected virtual bool HandleConditionalStep(ConditionalStep conditional)
        {
            var result = HandleSpeculation(conditional.Condition);

            if (result == conditional.Expecting)
            {
                if (conditional.HasThenStep)
                    return Walk(conditional.Then);
                else
                    return true;
            }
            else
            {
                if (conditional.HasElseStep)
                    return Walk(conditional.Else);
                else
                    return false;
            }

        }

        protected virtual bool HandleSpeculation(IStep speculative)
        {
            return Walk(speculative);
        }

        protected override IStep HandleNullStep()
            => throw new NullReferenceException("Step cannot be null");
    }
}