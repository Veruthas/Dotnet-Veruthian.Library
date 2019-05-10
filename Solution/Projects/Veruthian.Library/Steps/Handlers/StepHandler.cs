using System;

namespace Veruthian.Library.Steps.Handlers
{
    public abstract class StepHandler<TState, TStep> : IStepHandler<TState>
        where TStep : IStep
    {
        public virtual bool? Handle(IStep step, TState state, IStepHandler<TState> root = null)
        {
            switch (step)
            {
                case TStep tstep:
                    {
                        OnStepStarted(tstep, state);

                        var result = HandleStep(tstep, state, root);

                        OnStepCompleted(tstep, state, result);

                        return result;
                    }
                default:
                    {
                        return null;
                    }
            }
        }

        protected abstract bool? HandleStep(TStep step, TState state, IStepHandler<TState> root);


        protected virtual void OnStepStarted(TStep step, TState state)
        {
            if (StepStarted != null)
                StepStarted(step, state);
        }

        protected virtual void OnStepCompleted(TStep step, TState state, bool? result)
        {
            if (StepCompleted != null)
                StepCompleted(step, state, result);
        }


        public event Action<TStep, TState> StepStarted;

        public event Action<TStep, TState, bool?> StepCompleted;
    }
}