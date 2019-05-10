using System.Collections.Generic;

namespace Veruthian.Library.Steps.Handlers
{
    public class StepHandlers<TState> : TypedStepHandler<TState, IStep>
    {
        IEnumerable<IStepHandler<TState>> handlers;


        public StepHandlers(params IStepHandler<TState>[] handlers)
            : this((IEnumerable<IStepHandler<TState>>)handlers) { }

        public StepHandlers(IEnumerable<IStepHandler<TState>> handlers)
            => this.handlers = handlers ?? new IStepHandler<TState>[0];


        public override bool? Handle(IStep step, TState state, IStepHandler<TState> root)
        {
            bool? result = null;

            OnStepStarted(step, state);

            foreach (var handler in handlers)
            {
                result = handler.Handle(step, state, root);

                if (result != null)
                    break;
            }

            OnStepCompleted(step, state, result);

            return null;
        }


        protected override bool? HandleStep(IStep step, TState state, IStepHandler<TState> root) => null;


        public static StepHandlers<TState> GetBasic(BooleanStepHandler<TState> boolean = null,
                                                    SequentialStepHandler<TState> sequential = null,
                                                    OptionalStepHandler<TState> optional = null,
                                                    RepeatedStepHandler<TState> repeated = null,
                                                    RepeatedTryStepHandler<TState> repeatedTry = null,
                                                    ConditionalStepHandler<TState> conditional = null)
        {
            return new StepHandlers<TState>(boolean ?? new BooleanStepHandler<TState>(),
                                            sequential ?? new SequentialStepHandler<TState>(),
                                            optional ?? new OptionalStepHandler<TState>(),
                                            repeated ?? new RepeatedStepHandler<TState>(),
                                            repeatedTry ?? new RepeatedTryStepHandler<TState>(),
                                            conditional ?? new ConditionalStepHandler<TState>());
        }
    }


    public class BooleanStepHandler<TState> : TypedStepHandler<TState, BooleanStep>
    {
        protected override bool? HandleStep(BooleanStep step, TState state, IStepHandler<TState> root)
        {
            return step.Value;
        }
    }

    public class SequentialStepHandler<TState> : TypedStepHandler<TState, SequentialStep>
    {
        protected override bool? HandleStep(SequentialStep step, TState state, IStepHandler<TState> root)
        {
            foreach (var substep in step.Sequence)
            {
                var result = root.Handle(substep, state, root);

                if (result != true)
                    return result;
            }

            return true;
        }
    }

    public class OptionalStepHandler<TState> : TypedStepHandler<TState, OptionalStep>
    {
        protected override bool? HandleStep(OptionalStep step, TState state, IStepHandler<TState> root)
        {
            root.Handle(step.Step, state, root);

            return true;
        }
    }

    public class RepeatedStepHandler<TState> : TypedStepHandler<TState, RepeatedStep>
    {
        protected override bool? HandleStep(RepeatedStep step, TState state, IStepHandler<TState> root)
        {
            for (int i = 0; i < step.Count; i++)
            {
                var result = root.Handle(step.Step, state, root);

                if (result != true)
                    return false;
            }

            return true;
        }
    }

    public class RepeatedTryStepHandler<TState> : TypedStepHandler<TState, RepeatedTryStep>
    {
        protected override bool? HandleStep(RepeatedTryStep step, TState state, IStepHandler<TState> root)
        {
            if (step.Count == null)
            {
                while (root.Handle(step.Step, state, root) == true)
                    ;
            }
            else
            {
                for (int i = 0; i < step.Count; i++)
                    if (root.Handle(step.Step, state, root) != true)
                        break;
            }

            return true;
        }
    }

    public class ConditionalStepHandler<TState> : TypedStepHandler<TState, ConditionalStep>
    {
        protected override bool? HandleStep(ConditionalStep step, TState state, IStepHandler<TState> root)
        {
            var result = HandleSpeculation(step.Condition, state, root);

            if (result == step.Expecting)
            {
                if (step.HasThenStep)
                    return root.Handle(step.Then, state, root);
                else
                    return true;
            }
            else if (result == !step.Expecting)
            {
                if (step.HasElseStep)
                    return root.Handle(step.Else, state, root);
                else
                    return false;
            }
            else
            {
                return null;
            }
        }

        protected virtual bool? HandleSpeculation(IStep speculation, TState state, IStepHandler<TState> root)
        {
            OnSpeculationStarted(speculation, state);

            var result = root.Handle(speculation, state, root);

            OnSpeculationCompleted(speculation, state, result);

            return result;
        }


        protected virtual void OnSpeculationStarted(IStep speculation, TState state) { }

        protected virtual void OnSpeculationCompleted(IStep speculation, TState state, bool? result) { }
    }

}