using System;

namespace Veruthian.Library.Steps.Handlers
{
    public abstract class StepHandler<TStep> : IStepHandler
        where TStep : IStep
    {
        public virtual bool? Handle(IStep step, StateTable state, IStepHandler root = null)
        {
            switch (step)
            {
                case TStep tstep:
                    {
                        var result = OnStepStarted(tstep, state);

                        var handled = result == null;

                        if (!handled)
                            result = HandleStep(tstep, state, root ?? this);


                        OnStepCompleted(tstep, state, result, handled);


                        return result;
                    }
                default:
                    {
                        return null;
                    }
            }
        }

        protected abstract bool? HandleStep(TStep step, StateTable state, IStepHandler root);


        protected virtual bool? OnStepStarted(TStep step, StateTable state)
        {
            if (StepStarted != null)
                return StepStarted(step, state);

            return null;
        }

        protected virtual void OnStepCompleted(TStep step, StateTable state, bool? result, bool handled)
        {
            if (StepCompleted != null)
                StepCompleted(step, state, result, handled);
        }


        public event Func<TStep, StateTable, bool?> StepStarted;

        public event Action<TStep, StateTable, bool?, bool> StepCompleted;
    }
}